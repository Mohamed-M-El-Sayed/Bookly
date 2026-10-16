namespace Bookly.Application.Features.Favorites.Queries.GetMyFavorites
{
    public record FavoriteResponse(int Id, int HotelId, string HotelName, string HotelImage);
}
