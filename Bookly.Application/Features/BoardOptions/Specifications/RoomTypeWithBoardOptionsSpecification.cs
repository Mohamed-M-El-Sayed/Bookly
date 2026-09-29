using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.BoardOptions.Specifications;

public class RoomTypeWithBoardOptionsSpecification : BaseSpecification<RoomType>
{
    public RoomTypeWithBoardOptionsSpecification(int roomTypeId)
    {
        Criteria = x => x.Id == roomTypeId;
        AddInclude(x => x.BoardOptions);
    }
}
