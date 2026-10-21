using Bookly.Application.Features.Hotels.Queries;
using Bookly.Application.Features.Hotels.Queries.GetHotelAmenities;
using Bookly.Domain.Entities.Hotels;
using Mapster;

namespace Bookly.Application.Features.Hotels.Mapping;

public class HotelMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Hotel, SearchHotelsResponse>()
            .Map(dest => dest.City, src => src.Address.City)
            .Map(dest => dest.Country, src => src.Address.Country)
            .Map(dest => dest.PrimaryImageUrl, src => src.Images.Where(i => i.IsPrimary).Select(i => i.Url).FirstOrDefault())
            .Map(dest => dest.MinPricePerNight, src => src.RoomTypes.Min(rt => rt.BasePricePerNight));
        // this is wrong (may be have seasonl price)
        config.NewConfig<HotelAmenity, HotelAmenityResponse>()
            .Map(dest => dest.CategoryId, src => src.Amenity.CategoryId)
            .Map(dest => dest.NameEn, src => src.Amenity.NameEn)
            .Map(dest => dest.NameAr, src => src.Amenity.NameAr)
            .Map(dest => dest.Icon, src => src.Amenity.Icon)
            .Map(dest => dest.IsActive, src => src.Amenity.IsActive)
            .Map(dest => dest.CategoryNameEn, src => src.Amenity.Category.NameEn)
            .Map(dest => dest.CategoryNameAr, src => src.Amenity.Category.NameAr)
            .Map(dest => dest.Scope, src => src.Amenity.Scope);
    }
}
