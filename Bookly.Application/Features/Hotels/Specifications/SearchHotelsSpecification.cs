using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.RoomTypes;
using System.Linq.Expressions;

namespace Bookly.Application.Features.Hotels.Specifications;

public class SearchHotelsSpecification : BaseSpecification<Hotel>
{
    public Expression<Func<RoomType, bool>> RoomTypeFilter { get; }

    public SearchHotelsSpecification(
        string location,
        int guests,
        DateOnly checkIn,
        DateOnly checkOut,
        int pageIndex,
        int pageSize)
    {
        var nights = checkOut.DayNumber - checkIn.DayNumber;

        RoomTypeFilter = rt =>
            rt.MaxOccupancy >= guests
            && rt.RoomAvailabilities.Count(ra =>
                ra.Date >= checkIn &&
                ra.Date < checkOut &&
                ra.TotalRooms > ra.ReservedRooms) == nights;

        Criteria = h =>
            (string.IsNullOrEmpty(location)
                || h.Address.City.Contains(location)
                || h.Address.Country.Contains(location))
            && h.RoomTypes.AsQueryable().Any(RoomTypeFilter);

        AddInclude(h => h.Images.Where(i => i.IsPrimary));
        ApplyPagination(pageIndex, pageSize);
    }
}