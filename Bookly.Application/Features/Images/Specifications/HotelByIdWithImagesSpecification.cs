using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.HotelImages.Specifications;

public class HotelByIdWithImagesSpecification : BaseSpecification<Hotel>
{
    public HotelByIdWithImagesSpecification(int hotelId)
    {
        Criteria = x => x.Id == hotelId;
        AddInclude(x => x.Images);
    }
}
