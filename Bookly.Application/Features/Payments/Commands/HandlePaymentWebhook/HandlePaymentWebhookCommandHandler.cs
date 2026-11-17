using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Application.Contracts.Services.Payments;
using Bookly.Application.Features.Bookings.Commands.ReleaseBookingAvailability;
using Bookly.Application.Features.Payments.Messages;
using Bookly.Application.Features.Payments.Specifications;
using Bookly.Domain.Entities.Payments;
using Bookly.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;

public class HandlePaymentWebhookCommandHandler(IUnitOfWork unitOfWork,
    IPaymentService paymentService,
    IMediator mediator,
    ILogger<HandlePaymentWebhookCommandHandler> logger,
    IBackgroundJobService backgroundJobService) : IRequestHandler<HandlePaymentWebhookCommand, Result>
{
    public async Task<Result> Handle(HandlePaymentWebhookCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing payment webhook.");

        var webhookResult = paymentService.ParseWebhookEvent(
            request.Payload,
            request.Signature);

        if (!webhookResult.IsSuccess)
            return Result.Failure(webhookResult.Error!);

        var webhook = webhookResult.Value;

        var payment = await unitOfWork.Repository<Payment>()
            .GetEntityWithSpecAsync(
                new PaymentBySessionOrTransactionSpecification(
                    webhook.SessionId,
                    webhook.TransactionId));

        if (payment is null)
            return Result.Failure(PaymentErrors.NotFound());


        if (webhook.EventType != PaymentEventType.PaymentRefunded &&
            payment.Amount != webhook.Amount)
        {
            return Result.Failure(PaymentErrors.AmountMisMatch());
        }

        var booking = payment.Booking;

        switch (webhook.EventType)
        {
            case PaymentEventType.PaymentSucceeded:

                if (payment.Status == PaymentStatus.Paid)
                    return Result.Success();
                payment.MarkAsPaid(webhook.TransactionId!);
                payment.Booking.Confirm();
                backgroundJobService.Enqueue<IBookingConfirmationEmailJob>(
                    job => job.SendAsync(payment.BookingId, CancellationToken.None));
                break;

            case PaymentEventType.PaymentFailed:

                if (payment.Status == PaymentStatus.Failed)
                    return Result.Success();

                payment.MarkAsFailed();
                var releaseResult = await mediator.Send(new ReleaseBookingAvailabilityCommand(booking.Id), cancellationToken);

                if (releaseResult.IsFailure)
                    return releaseResult;

                break;

            case PaymentEventType.PaymentRefunded:

                if (payment.Status == PaymentStatus.Refunded)
                    return Result.Success();

                if (booking.Cancellation is null)
                {
                    logger.LogWarning("Refund received for Payment {PaymentId}, but Booking {BookingId} has no cancellation.",
                        payment.Id,
                        booking.Id);

                    return Result.Failure(
                        PaymentErrors.CancellationNotFound());
                }
                booking.Cancel();
                payment.MarkAsRefunded(webhook.Amount, webhook.TransactionId!, DateTime.UtcNow);
                booking.Cancellation.MarkRefundAsSucceeded();
                releaseResult = await mediator.Send(new ReleaseBookingAvailabilityCommand(booking.Id), cancellationToken);

                if (releaseResult.IsFailure)
                    return releaseResult;
                break;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Payment webhook processed successfully. PaymentId: {PaymentId}, EventType: {EventType}",
            payment.Id,
            webhook.EventType);

        return Result.Success();
    }
}