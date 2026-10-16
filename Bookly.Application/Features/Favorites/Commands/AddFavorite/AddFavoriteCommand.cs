using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Favorites.Commands.AddFavorite;

public record AddFavoriteCommand(int HotelId) : IRequest<Result>;


