using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Bookings.Specifications;

public class RoomTypeForBookingSpecification : BaseSpecification<RoomType>
{
    public RoomTypeForBookingSpecification(int roomTypeId, DateOnly checkIn, DateOnly checkOut)
    {
        Criteria = r => r.Id == roomTypeId;
        AddInclude(x => x.RoomAvailabilities.Where(
            a => a.Date >= checkIn && a.Date < checkOut));
        AddInclude(x => x.SeasonalPrices);
    }
}
