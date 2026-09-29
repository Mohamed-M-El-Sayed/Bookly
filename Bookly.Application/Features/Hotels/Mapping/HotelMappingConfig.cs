using Bookly.Application.Features.Hotels.Queries;
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
    }
}
