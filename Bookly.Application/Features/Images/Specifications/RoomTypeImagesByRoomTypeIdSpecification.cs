using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Images.Specifications;

public class RoomTypeImagesByRoomTypeIdSpecification : BaseSpecification<RoomTypeImage>
{
    public RoomTypeImagesByRoomTypeIdSpecification(int roomTypeId)
    {
        Criteria = x => x.RoomTypeId == roomTypeId;
    }
}
