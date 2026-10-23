using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.Specifications;

public class HotelAmenitiesSpecifications : BaseSpecification<HotelAmenity>
{
    public HotelAmenitiesSpecifications(int hotelId, bool includeInactive = false)
    {
        // Include only active amenities in active categories for public display.
        // Set includeInactive to true to include all assigned amenities for management.
        Criteria = ha =>
            ha.HotelId == hotelId &&
            (includeInactive ||
             (ha.Amenity.IsActive && ha.Amenity.Category.IsActive));
        AddInclude(ha => ha.Amenity.Category);
    }
}