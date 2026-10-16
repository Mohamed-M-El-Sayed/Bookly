using Bookly.Application.Features.Favorites.Queries.GetMyFavorites;
using Bookly.Domain.Entities.Favorites;
using Mapster;

namespace Bookly.Application.Features.Favorites.Mapping;

public class FavoriteMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Favorite, FavoriteResponse>()
            .Map(dest => dest.HotelId, src => src.HotelId)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.HotelImage, src =>
                src.Hotel.Images.Where(i => i.IsPrimary).Select(i => i.Url).FirstOrDefault());
    }
}
