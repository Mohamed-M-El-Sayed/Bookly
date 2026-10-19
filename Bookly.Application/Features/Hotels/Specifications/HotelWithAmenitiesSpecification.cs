using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.Specifications;

public class HotelWithAmenitiesSpecification : BaseSpecification<Hotel>
{
    public HotelWithAmenitiesSpecification(int hotelId)
    {
        Criteria = h => h.Id == hotelId;
        AddInclude(h => h.Amenities);
    }
}
