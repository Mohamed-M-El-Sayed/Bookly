using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Domain.Entities.Cancellation;

public class BookingCancellationRule : BaseEntity
{
    // A snapshot of the cancellation rules is stored when a new booking is created.
    public int BookingId { get; set; }
    public int DaysBeforeCheckIn { get; private set; }
    public decimal RefundPercentage { get; private set; }
    public Booking Booking { get; private set; } = null!;
    private BookingCancellationRule() { }
}
