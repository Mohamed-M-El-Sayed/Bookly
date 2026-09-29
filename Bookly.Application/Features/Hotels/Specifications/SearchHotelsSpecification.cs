using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.Specifications;

public class SearchHotelsSpecification : BaseSpecification<Hotel>
{
    public SearchHotelsSpecification(string location, int guests, DateOnly checkIn, DateOnly checkOut,
        int pageIndex, int pageSize)
    {
        Criteria = h =>
              (string.IsNullOrEmpty(location)
                  || h.Address.City.Contains(location)
                  || h.Address.Country.Contains(location))
              && h.RoomTypes.Any(rt =>
                  rt.MaxOccupancy >= guests
                  && !rt.RoomAvailabilities.Any(ra =>
                      ra.Date >= checkIn
                      && ra.Date < checkOut
                      && ra.AvailableRooms <= 0));

        AddInclude(h => h.Images.Where(i => i.IsPrimary));
        // AddInclude(h => h.Address);
        AddInclude(h => h.RoomTypes);
        ApplyPagination(pageIndex, pageSize);
    }
}
