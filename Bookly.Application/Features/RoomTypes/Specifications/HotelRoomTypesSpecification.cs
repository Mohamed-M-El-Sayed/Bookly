using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.RoomTypes.Specifications;

public class HotelRoomTypesSpecification : BaseSpecification<RoomType>
{
    public HotelRoomTypesSpecification(int hotelId)
    {
        Criteria = x => x.HotelId == hotelId;
    }
}
