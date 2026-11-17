using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.HotelImages.Specifications
{
    public class HotelImagesByHotelIdSpecification : BaseSpecification<HotelImage>
    {
        public HotelImagesByHotelIdSpecification(int hotelId)
        {
            Criteria = x => x.HotelId == hotelId;
        }
    }
}
