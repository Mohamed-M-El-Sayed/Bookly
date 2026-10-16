using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Favorites.Commands.ClearMyFavorites;

public record ClearMyFavoritesCommand : IRequest<Result>;
