using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Enums;

namespace Bookly.Domain.Entities.Payments;

public class Payment : SoftDeletableEntity
{
    public int BookingId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? PaymentSessionId { get; private set; }
    public string? TransactionId { get; private set; }
    // Refund information
    public decimal? RefundAmount { get; private set; }
    public DateTime? RefundedAt { get; private set; }
    public string? RefundTransactionId { get; private set; }
    public Booking Booking { get; private set; } = default!;
    private Payment() { }
    public Payment(int bookingId, decimal amount)
    {
        BookingId = bookingId;
        Amount = amount;
        Status = PaymentStatus.Pending;
    }
    public void SetPaymentSessionId(string sessionId)
    {
        PaymentSessionId = sessionId;
    }
}