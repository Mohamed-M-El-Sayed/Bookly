using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Favorites.Queries.GetMyFavorites;

public record GetMyFavoritesQuery : IRequest<Result<IReadOnlyList<FavoriteResponse>>>;

