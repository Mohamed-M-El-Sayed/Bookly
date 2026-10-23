using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.RoomTypes.Specifications;

public class RoomTypeAmenitiesSpecification : BaseSpecification<RoomTypeAmenity>
{
    public RoomTypeAmenitiesSpecification(int roomTypeId, bool includeInactive = false)
    {
        // Include only active amenities in active categories for public display.
        // Set includeInactive to true for management and unassignment.
        Criteria = ra =>
            ra.RoomTypeId == roomTypeId &&
            (includeInactive ||
             (ra.Amenity.IsActive && ra.Amenity.Category.IsActive));
        AddInclude(ra => ra.Amenity.Category);
    }
}