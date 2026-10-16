
using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Favorites.Errors;

public static class FavoriteErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("Favorite.NotFound", "The favorite hotel was not found.");
    public static Error AlreadyExists() =>
           ErrorFactory.Conflict("Favorite.AlreadyExists", "This hotel is already in your favorites.");
}
