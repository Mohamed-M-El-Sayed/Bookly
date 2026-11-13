using Bookly.Application.Common.Mappings;
using Bookly.Application.Features.RoomTypes.Queries.GetAvailableHotelRoomTypes;
using Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeAmenities;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;

namespace Bookly.Application.Features.RoomTypes.Mapping;

public class RoomTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RoomTypeAmenity, RoomTypeAmenityResponse>()
               .Map(dest => dest.AmenityId, src => src.AmenityId)
               .Map(dest => dest.NameEn, src => src.Amenity.NameEn)
               .Map(dest => dest.NameAr, src => src.Amenity.NameAr)
               .Map(dest => dest.Icon, src => src.Amenity.Icon)
               .Map(dest => dest.IsActive, src => src.Amenity.IsActive)
               .Map(dest => dest.CategoryId, src => src.Amenity.Category.Id)
               .Map(dest => dest.CategoryNameEn, src => src.Amenity.Category.NameEn)
               .Map(dest => dest.CategoryNameAr, src => src.Amenity.Category.NameAr);
        config.NewConfig<RoomTypeImage, RoomTypeImageResponse>()
               .Map(dest => dest.Id, src => src.Id)
               .Map(dest => dest.Url, src => src.Url.ToFullImageUrl())
               .Map(dest => dest.IsPrimary, src => src.IsPrimary);
    }
}
