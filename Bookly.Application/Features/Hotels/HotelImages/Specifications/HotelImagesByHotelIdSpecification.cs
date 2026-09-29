using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.Hotels.HotelImages.Specifications
{
    public class HotelImagesByHotelIdSpecification : BaseSpecification<HotelImage>
    {
        public HotelImagesByHotelIdSpecification(int hotelId)
        {
            Criteria = x => x.HotelId == hotelId;
        }
    }
}
