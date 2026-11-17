using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Payments;

namespace Bookly.Application.Features.Payments.Specifications;

public class PaymentBySessionOrTransactionSpecification : BaseSpecification<Payment>
{
    public PaymentBySessionOrTransactionSpecification(string? sessionId, string? transactionId)
    {
        Criteria = p =>
            (!string.IsNullOrEmpty(sessionId) &&
             p.PaymentSessionId == sessionId) ||
            (!string.IsNullOrEmpty(transactionId) &&
             p.TransactionId == transactionId);
        AddInclude(p => p.Booking.BookingRooms);
        AddInclude(p => p.Booking.Cancellation!);
    }
}
