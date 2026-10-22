using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Amenities.Specification;

public class HotelsUsingAmenitySpecification : BaseSpecification<Hotel>
{
    public HotelsUsingAmenitySpecification(int amenityId)
    {
        Criteria = h => h.Amenities.Any(ha => ha.AmenityId == amenityId);
    }
}
