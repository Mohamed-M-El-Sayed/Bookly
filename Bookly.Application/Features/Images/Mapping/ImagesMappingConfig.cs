using Bookly.Application.Common.Mappings;
using Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;
using Bookly.Domain.Entities.Hotels;
using Mapster;

namespace Bookly.Application.Features.Images.Mapping
{
    public class ImagesMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<HotelImage, HotelImagesResponse>()
                .Map(dest => dest.Url, src => src.Url.ToFullImageUrl());
        }
    }
}
