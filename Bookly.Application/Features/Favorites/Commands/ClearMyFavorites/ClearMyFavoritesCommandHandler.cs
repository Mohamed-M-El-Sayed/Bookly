using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Favorites.Specifications;
using Bookly.Domain.Entities.Favorites;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Favorites.Commands.ClearMyFavorites
{
    public class ClearMyFavoritesCommandHandler(IUnitOfWork unitOfWork,
        ILogger<ClearMyFavoritesCommandHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<ClearMyFavoritesCommand, Result>
    {
        public async Task<Result> Handle(ClearMyFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId is null)
                return Result.Failure(AuthErrors.Unauthorized());
            var favorites = await unitOfWork.Repository<Favorite>()
                .GetAllWithSpecAsync(new GetMyFavoritesSpecification(userId.Value));
            unitOfWork.Repository<Favorite>().RemoveRange(favorites);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation(
                "User {UserId} cleared {Count} favorites",
                userId.Value,
                favorites.Count());
            return Result.Success();
        }
    }
}
