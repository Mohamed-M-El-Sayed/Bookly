using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Favorites.Errors;
using Bookly.Domain.Entities.Favorites;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Favorites.Commands.RemoveFavorite;

public class RemoveFavoriteCommandHandler(IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<RemoveFavoriteCommandHandler> logger) : IRequestHandler<RemoveFavoriteCommand, Result>
{
    public async Task<Result> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
        {
            return Result.Failure(AuthErrors.Unauthorized());
        }
        var favorite = await unitOfWork.Repository<Favorite>()
            .FindAsync(f => f.UserId == userId.Value && f.HotelId == request.HotelId, cancellationToken);
        if (favorite is null)
        {
            return Result.Failure(FavoriteErrors.NotFound());
        }
        unitOfWork.Repository<Favorite>().Remove(favorite);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "User {UserId} removed hotel {HotelId} from favorites",
            userId.Value,
            request.HotelId);
        return Result.Success();
    }
}
