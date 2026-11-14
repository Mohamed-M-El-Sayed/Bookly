using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.BoardOptions.Errors;
using Bookly.Application.Features.BoardOptions.Specifications;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.BoardOptions.Commands.UpdateBoardOption;

public class UpdateBoardOptionCommandHandler(
    IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<UpdateBoardOptionCommandHandler> logger)
    : IRequestHandler<UpdateBoardOptionCommand, Result>
{
    public async Task<Result> Handle(UpdateBoardOptionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating board option {BoardOptionId} for room type {RoomTypeId}",
            request.BoardOptionId, request.RoomTypeId);

        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(
                new RoomTypeWithBoardOptionsSpecification(request.RoomTypeId), cancellationToken);
        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(
            roomType.HotelId, cancellationToken);
        if (accessResult.IsFailure)
            return Result.Failure(accessResult.Error!);

        if (!roomType.UpdateBoardOption(request.BoardOptionId, request.Type, request.ExtraPricePerNight))
        {
            logger.LogWarning("Board option {BoardOptionId} was not found for room type {RoomTypeId}",
                request.BoardOptionId, request.RoomTypeId);
            return Result.Failure(BoardOptionErrors.NotFound());
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Board option {BoardOptionId} was successfully updated", request.BoardOptionId);
        return Result.Success();
    }
}
