using Bookly.Domain.Entities.Base;
using Bookly.Domain.Enums;
using Bookly.Domain.Exceptions;

namespace Bookly.Domain.Entities.Bookings;

public class BookingCancellation : SoftDeletableEntity
{
    public int BookingId { get; private set; }
    public DateTime CancelledAt { get; private set; }
    public string? Reason { get; private set; }
    public decimal CancellationFee { get; private set; }
    public decimal RefundAmount { get; private set; }
    public Booking Booking { get; private set; } = null!;
    public RefundStatus RefundStatus { get; private set; }
    private BookingCancellation() { }

    private BookingCancellation(
        int bookingId,
        DateTime cancelledAt,
        string? reason,
        decimal refundAmount,
        decimal cancellationFee)
    {
        BookingId = bookingId;
        CancelledAt = cancelledAt;
        Reason = reason;
        RefundAmount = refundAmount;
        CancellationFee = cancellationFee;
        RefundStatus = RefundStatus.Pending;
    }

    public static BookingCancellation Create(
        int bookingId,
        DateTime cancelledAt,
        string? reason,
        decimal totalAmount,
        decimal refundAmount)
    {

        if (refundAmount > totalAmount)
            throw new ArgumentOutOfRangeException(
                nameof(refundAmount),
                "Refund amount cannot exceed the booking total.");

        var cancellationFee = totalAmount - refundAmount;

        return new BookingCancellation(
            bookingId,
            cancelledAt,
            reason,
            refundAmount,
            cancellationFee);
    }
    public void MarkRefundAsFailed()
    {
        if (RefundStatus != RefundStatus.Pending)
            throw new DomainException("Refund is not pending.");

        RefundStatus = RefundStatus.Failed;
    }
    public void RetryRefund()
    {
        if (RefundStatus != RefundStatus.Failed)
            throw new DomainException("Only failed refunds can be retried.");
        RefundStatus = RefundStatus.Pending;
    }
}