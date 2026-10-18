using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Amenities;

namespace Bookly.Application.Features.AmenityCategories.Specifications;

public class AmenityCategoriesSpecification : BaseSpecification<AmenityCategory>
{
    public AmenityCategoriesSpecification(bool includeInactive)
    {
        // Hotel managers should only see active categories when viewing available categories.
        Criteria = x => includeInactive || x.IsActive;
        ApplyOrderBy(x => x.SortOrder);
    }
}