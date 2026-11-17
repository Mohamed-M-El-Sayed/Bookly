using Bookly.Application.Common.Mappings;
using Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;
using Bookly.Application.Features.Images.Queries.GetAllRoomTypeImages;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;

namespace Bookly.Application.Features.Images.Mapping;

public class ImagesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<HotelImage, HotelImagesResponse>()
            .Map(dest => dest.Url, src => src.Url.ToFullImageUrl());
        config.NewConfig<RoomTypeImage, RoomTypeImagesResponse>()
            .Map(dest => dest.Url, src => src.Url.ToFullImageUrl());
    }
}
