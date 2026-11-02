using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Application.Features.Bookings.Specifications;

public class BookingWithBookingRoomSpecification : BaseSpecification<Booking>
{
    public BookingWithBookingRoomSpecification(int bookingId)
    {
        Criteria = b => b.Id == bookingId;
        AddInclude(b => b.BookingRooms);
    }
}
