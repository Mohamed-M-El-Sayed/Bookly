using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.Specifications;

public class HotelAmenitiesSpecifications : BaseSpecification<HotelAmenity>
{
    public HotelAmenitiesSpecifications(int hotelId)
    {
        Criteria = ha => ha.HotelId == hotelId && ha.Amenity.IsActive;
        AddInclude(ha => ha.Amenity.Category);
    }
}
