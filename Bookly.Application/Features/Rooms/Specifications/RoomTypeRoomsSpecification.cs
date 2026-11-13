using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Rooms.Specifications;

public class RoomTypeRoomsSpecification : BaseSpecification<Room>
{
    public RoomTypeRoomsSpecification(int roomTypeId, int pageIndex, int pageSize)
    {
        Criteria = room => room.RoomTypeId == roomTypeId;
        ApplyPagination(pageIndex, pageSize);
    }
}
