using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Favorites.Commands.RemoveFavorite;

public record RemoveFavoriteCommand(int HotelId) : IRequest<Result>;
