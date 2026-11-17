using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.RoomTypes.Specifications;

public class AvailableRoomTypesSpecification : BaseSpecification<RoomType>
{
    public AvailableRoomTypesSpecification(int hotelId, DateOnly checkIn, DateOnly checkOut, int guests, int pageIndex, int pageSize)
    {
        Criteria = roomType => roomType.HotelId == hotelId
        && roomType.MaxOccupancy >= guests
        && roomType.RoomAvailabilities
              .Where(x =>
                  x.Date >= checkIn &&
                  x.Date < checkOut)
              .Count() == checkOut.DayNumber - checkIn.DayNumber;
        AddInclude(rt => rt.SeasonalPrices);
        AddInclude(rt => rt.Images);
        AddInclude(rt => rt.BoardOptions.Where(x => x.IsActive));
        ApplyPagination(pageIndex, pageSize);
    }
}
