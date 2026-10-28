using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Application.Features.Bookings.Specifications;

public class BookingForExpirySpecification : BaseSpecification<Booking>
{
    public BookingForExpirySpecification(int bookingId)
    {
        Criteria = b => b.Id == bookingId;
        AddInclude(b => b.BookingRooms);
    }
}
