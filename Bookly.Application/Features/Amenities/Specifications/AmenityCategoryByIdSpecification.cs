using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Amenities;

namespace Bookly.Application.Features.Amenities.Specifications;

public class AmenityCategoryByIdSpecification : BaseSpecification<AmenityCategory>
{
    public AmenityCategoryByIdSpecification(int id)
    {
        Criteria = c => c.Id == id;
    }
}
