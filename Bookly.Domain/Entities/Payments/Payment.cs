using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Enums;
using Bookly.Domain.Exceptions;

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
    public void MarkAsPaid(string transactionId)
    {
        if (Status == PaymentStatus.Paid)
            return;

        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payments can be marked as paid.");

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new DomainException("Transaction ID is required.");

        TransactionId = transactionId;
        Status = PaymentStatus.Paid;
    }
    public void MarkAsFailed()
    {
        if (Status == PaymentStatus.Failed)
            return;
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payments can be marked as failed.");
        Status = PaymentStatus.Failed;
    }
}