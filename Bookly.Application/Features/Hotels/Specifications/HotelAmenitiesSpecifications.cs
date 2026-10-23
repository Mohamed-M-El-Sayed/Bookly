using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.Specifications;

public class HotelAmenitiesSpecifications : BaseSpecification<HotelAmenity>
{
    public HotelAmenitiesSpecifications(int hotelId)
    {
        // Include inactive amenities already assigned to the hotel
        // so the hotel manager can view and unassign them.
        Criteria = ha => ha.HotelId == hotelId;
        AddInclude(ha => ha.Amenity.Category);
    }
}
