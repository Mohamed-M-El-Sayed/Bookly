using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Favorites.Errors;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Favorites;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Favorites.Commands.AddFavorite;

public class AddFavoriteCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<AddFavoriteCommandHandler> logger)
    : IRequestHandler<AddFavoriteCommand, Result>
{
    public async Task<Result> Handle(
        AddFavoriteCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is null)
        {
            return Result.Failure(AuthErrors.Unauthorized());
        }

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
            return Result.Failure(HotelErrors.NotFound());


        var existingFavorite = await unitOfWork.Repository<Favorite>()
            .FindAsync(
                f => f.UserId == userId.Value &&
                     f.HotelId == request.HotelId,
                cancellationToken);

        if (existingFavorite is not null)
            return Result.Failure(FavoriteErrors.AlreadyExists());


        var favorite = new Favorite(userId.Value, request.HotelId);

        await unitOfWork.Repository<Favorite>()
            .AddAsync(favorite, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "User {UserId} added hotel {HotelId} to favorites",
            userId.Value,
            request.HotelId);

        return Result.Success();
    }
}