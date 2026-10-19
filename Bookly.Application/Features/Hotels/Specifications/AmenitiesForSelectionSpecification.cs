using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Amenities;

namespace Bookly.Application.Features.Hotels.Specifications
{
    public class AmenitiesForSelectionSpecification : BaseSpecification<Amenity>
    {
        public AmenitiesForSelectionSpecification(IEnumerable<int> amenityIds)
        {
            Criteria = a => amenityIds.Contains(a.Id) && a.IsActive;
        }
    }
}
