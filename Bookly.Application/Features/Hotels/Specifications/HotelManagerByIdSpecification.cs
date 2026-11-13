using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.Specifications;

public class HotelManagerByIdSpecification : BaseSpecification<HotelManager>
{
    public HotelManagerByIdSpecification(int hotelId, Guid userId)
    {
        Criteria = x => x.HotelId == hotelId && x.UserId == userId;
    }
}
