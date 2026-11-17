using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Images.Specifications;

public class RoomTypeByIdWithImagesSpecification : BaseSpecification<RoomType>
{
    public RoomTypeByIdWithImagesSpecification(int roomTypeId)
    {
        Criteria = x => x.Id == roomTypeId;
        AddInclude(x => x.Images);
    }
}
