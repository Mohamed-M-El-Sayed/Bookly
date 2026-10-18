using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Amenities;

namespace Bookly.Application.Features.Amenities.Specification;

public class AmenitiesSpecification : BaseSpecification<Amenity>
{
    public AmenitiesSpecification(int categoryId, bool includeInactive)
    {
        // Hotel managers should only see active categories when viewing available categories.
        Criteria = a => a.CategoryId == categoryId && (includeInactive || a.IsActive);
        ApplyOrderByDescending(a => a.IsActive);
    }
}
