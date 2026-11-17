using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;

public class BoardOptionsByRoomTypeIdSpecification : BaseSpecification<BoardOption>
{
    public BoardOptionsByRoomTypeIdSpecification(int roomTypeId)
    {
        Criteria = x => x.RoomTypeId == roomTypeId;
    }
}
