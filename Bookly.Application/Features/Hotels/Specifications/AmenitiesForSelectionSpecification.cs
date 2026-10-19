using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Hotels.Specifications
{
    public class AmenitiesForSelectionSpecification : BaseSpecification<Amenity>
    {
        public AmenitiesForSelectionSpecification(IEnumerable<int> amenityIds, AmenityScope scope)
        {
            Criteria = a => amenityIds.Contains(a.Id)
            && a.IsActive
            && (a.Scope == scope || a.Scope == AmenityScope.Both);
        }
    }
}
