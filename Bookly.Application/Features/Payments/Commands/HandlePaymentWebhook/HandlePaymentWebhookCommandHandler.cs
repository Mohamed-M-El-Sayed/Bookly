using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Application.Contracts.Services.Payments;
using Bookly.Application.Features.Payments.Messages;
using Bookly.Application.Features.Payments.Specifications;
using Bookly.Domain.Entities.Payments;
using Bookly.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;

public class HandlePaymentWebhookCommandHandler(IUnitOfWork unitOfWork,
    IPaymentService paymentService,
    IBackgroundJobService backgroundJobService,
    ILogger<HandlePaymentWebhookCommandHandler> logger) : IRequestHandler<HandlePaymentWebhookCommand, Result>
{
    public async Task<Result> Handle(HandlePaymentWebhookCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing payment webhook.");
        var webhookResult = paymentService.ParseWebhookEvent(request.Payload, request.Signature);
        if (!webhookResult.IsSuccess)
            return Result.Failure(webhookResult.Error!);
        var webhook = webhookResult.Value;
        var payment = await unitOfWork.Repository<Payment>().GetEntityWithSpecAsync(
            new PaymentBySessionOrTransactionSpecification(webhook.SessionId, webhook.TransactionId));
        if (payment is null)
            return Result.Failure(PaymentErrors.NotFound());
        if (payment.Amount != webhook.Amount)
            return Result.Failure(PaymentErrors.AmountMisMatch());
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
                payment.MarkAsFailed();
                var booking = payment.Booking;
                var released = booking.BookingRooms.Single().RoomType.TryRelease(booking.CheckIn, booking.CheckOut);
                if (!released)
                {
                    logger.LogWarning(
                        "Failed to release availability for Booking {BookingId}.",
                        booking.Id);
                }
                break;

            case PaymentEventType.PaymentRefunded:
                // Handle refund in the next step.
                break;
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Payment webhook processed successfully. PaymentId: {PaymentId}, EventType: {EventType}",
           payment.Id,
           webhook.EventType);
        return Result.Success();
    }
}
