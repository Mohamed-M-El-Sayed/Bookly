using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Favorites.Specifications;
using Bookly.Domain.Entities.Favorites;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Favorites.Queries.GetMyFavorites;

public class GetMyFavoritesQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetMyFavoritesQueryHandler> logger,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyFavoritesQuery, Result<IReadOnlyList<FavoriteResponse>>>
{
    public async Task<Result<IReadOnlyList<FavoriteResponse>>> Handle(GetMyFavoritesQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is null)
        {
            logger.LogWarning("Unauthenticated user tried to get favorites.");
            return Result<IReadOnlyList<FavoriteResponse>>.Failure(AuthErrors.Unauthorized());
        }
        var favorites = await unitOfWork.Repository<Favorite>()
            .GetAllWithSpecAsync(new GetMyFavoritesSpecification(userId.Value));
        var response = favorites.Adapt<IReadOnlyList<FavoriteResponse>>();
        logger.LogInformation(
            "Retrieved {Count} favorites for user {UserId}",
            response.Count,
            userId.Value);
        return Result<IReadOnlyList<FavoriteResponse>>.Success(response);
    }
}