using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Bookings;

public class BookingCancellation : SoftDeletableEntity
{
    public int BookingId { get; private set; }
    public DateTime CancelledAt { get; private set; }
    public string? Reason { get; private set; }
    public decimal CancellationFee { get; private set; }
    public decimal RefundAmount { get; private set; }
    public Booking Booking { get; private set; } = null!;
}
