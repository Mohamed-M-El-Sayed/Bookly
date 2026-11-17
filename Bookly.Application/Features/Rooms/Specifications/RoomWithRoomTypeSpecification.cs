using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Rooms.Specifications;

public class RoomWithRoomTypeSpecification : BaseSpecification<Room>
{
    public RoomWithRoomTypeSpecification(int id)
    {
        Criteria = room => room.Id == id;
        AddInclude(room => room.RoomType);
    }
}
