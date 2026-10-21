using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.RoomTypes.Specifications
{
    public class RoomTypeWithAmenitiesSpecification : BaseSpecification<RoomType>
    {
        public RoomTypeWithAmenitiesSpecification(int roomTypeId)
        {
            Criteria = r => r.Id == roomTypeId;
            AddInclude(r => r.Amenities);
        }
    }
}
