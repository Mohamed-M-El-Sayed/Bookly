using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Bookings.Specifications;

public class RoomTypeForBookingSpecification : BaseSpecification<RoomType>
{
    public RoomTypeForBookingSpecification(int roomTypeId, DateOnly checkIn, DateOnly checkOut, int? boardOptionId)
    {
        Criteria = r => r.Id == roomTypeId;
        AddInclude(x => x.RoomAvailabilities.Where(
            a => a.Date >= checkIn && a.Date < checkOut));
        AddInclude(x => x.SeasonalPrices.Where(s => s.StartDate <= checkOut && s.EndDate >= checkIn));
        if (boardOptionId.HasValue)
        {
            AddInclude(r => r.BoardOptions.Where(
                b => b.Id == boardOptionId.Value));
        }
    }
}
