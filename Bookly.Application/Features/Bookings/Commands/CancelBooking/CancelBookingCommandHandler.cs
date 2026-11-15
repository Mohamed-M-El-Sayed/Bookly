using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Payments;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Bookings.Messages;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
    IPaymentService paymentService,
    ILogger<CancelBookingCommandHandler> logger)
    : IRequestHandler<CancelBookingCommand, Result>
{
    public async Task<Result> Handle(
        CancelBookingCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is null)
            return Result.Failure(AuthErrors.Unauthorized());
        var booking = await unitOfWork.Repository<Booking>()
            .GetEntityWithSpecAsync(
                new BookingForCancellationSpecification(
                    request.BookingId,
                    userId.Value),
                cancellationToken);

        if (booking is null)
            return Result.Failure(BookingErrors.NotFound());

        if (booking.Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.CannotBeCancelled());

        var payment = booking.Payments
            .FirstOrDefault(p => p.Status == PaymentStatus.Paid);

        if (payment is null || string.IsNullOrWhiteSpace(payment.TransactionId))
            return Result.Failure(BookingErrors.CannotBeCancelled());

        var requestedAt = DateTime.UtcNow;
        var refundAmount = booking.CalculateRefundAmount(requestedAt);
        var cancellation = await unitOfWork.Repository<BookingCancellation>()
            .GetEntityWithSpecAsync(new BookingCancellationByBookingIdSpecification(booking.Id),
        cancellationToken);

        if (cancellation is not null)
        {
            if (cancellation.RefundStatus == RefundStatus.Pending)
                return Result.Failure(BookingErrors.CancellationAlreadyPending());

            if (cancellation.RefundStatus == RefundStatus.Succeeded)
                return Result.Failure(BookingErrors.CannotBeCancelled());

            cancellation.RetryRefund();
        }
        else
        {
            cancellation = BookingCancellation.Create(
                booking.Id,
                DateTime.UtcNow,
                request.Reason,
                booking.TotalPrice,
                refundAmount);

            await unitOfWork.Repository<BookingCancellation>()
                .AddAsync(cancellation, cancellationToken);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (refundAmount == 0)
        {
            logger.LogInformation(
                "Cancellation request for booking {BookingId} requires no refund.",
                booking.Id);
            return Result.Success();
        }
        var refundResult = await paymentService.RefundPaymentAsync(
            payment.TransactionId,
            refundAmount,
            $"booking-refund-{booking.Id}",
            cancellationToken);
        if (refundResult.IsFailure)
        {
            cancellation.MarkRefundAsFailed();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogWarning(
                "Refund request failed for booking {BookingId}.",
                booking.Id);

            return Result.Failure(refundResult.Error!);
        }
        logger.LogInformation(
            "Refund request submitted for booking {BookingId}. RefundId: {RefundId}",
            booking.Id,
            refundResult.ToString());
        return Result.Success();
    }
}