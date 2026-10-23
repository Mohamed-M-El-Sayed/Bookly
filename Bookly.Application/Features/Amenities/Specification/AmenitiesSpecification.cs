using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Amenities;

namespace Bookly.Application.Features.Amenities.Specification;

public class AmenitiesSpecification : BaseSpecification<Amenity>
{
    public AmenitiesSpecification(int categoryId, bool includeInactive)
    {
        // Hotel managers can only see active amenities in active categories.
        Criteria = a =>
                   a.CategoryId == categoryId &&
                   (includeInactive ||
                    (a.IsActive && a.Category.IsActive));
        ApplyOrderByDescending(a => a.IsActive);
    }
}
