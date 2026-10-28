using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Bookings.Specifications;

public class HotelBookingsSpecification : BaseSpecification<Booking>
{
    public HotelBookingsSpecification(int hotelId,
        int pageIndex,
        int pageSize,
        BookingStatus? status,
        DateOnly? checkInFrom,
        DateOnly? checkInTo)
    {
        Criteria = b => b.HotelId == hotelId
        && (status == null || b.Status == status)
        && (checkInFrom == null || b.CheckIn >= checkInFrom)
        && (checkInTo == null || b.CheckIn <= checkInTo);
        ApplyPagination(pageIndex, pageSize);
        AddInclude(b => b.BookingRooms);
    }
}
