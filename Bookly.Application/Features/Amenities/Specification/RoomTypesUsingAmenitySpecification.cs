using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Amenities.Specification;

public class RoomTypesUsingAmenitySpecification : BaseSpecification<RoomType>
{
    public RoomTypesUsingAmenitySpecification(int amenityId)
    {
        Criteria = r => r.Amenities.Any(ra => ra.AmenityId == amenityId);
    }
}
