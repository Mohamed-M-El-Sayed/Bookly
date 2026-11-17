using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Application.Features.Bookings.Specifications;

public class BookingForAvailabilityReleaseSpecification : BaseSpecification<Booking>
{
    public BookingForAvailabilityReleaseSpecification(int bookingId)
    {
        Criteria = b => b.Id == bookingId;
        AddInclude(b => b.BookingRooms);
        AddInclude("BookingRooms.RoomType");
        AddInclude("BookingRooms.RoomType.Availability");
    }
}
