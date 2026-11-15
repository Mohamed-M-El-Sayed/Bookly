using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Application.Features.Bookings.Specifications;

public class BookingForCancellationSpecification : BaseSpecification<Booking>
{
    public BookingForCancellationSpecification(int bookingId, Guid userId)
    {
        Criteria = booking =>
            booking.Id == bookingId &&
            booking.UserId == userId;
        AddInclude(b => b.BookingRooms);
        AddInclude(b => b.Payments);
        AddInclude(b => b.CancellationRules);
    }
}