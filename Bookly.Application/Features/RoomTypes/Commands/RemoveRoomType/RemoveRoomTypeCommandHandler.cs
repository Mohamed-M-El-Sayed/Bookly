using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Commands.RemoveRoomType;

public class RemoveRoomTypeCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<RemoveRoomTypeCommandHandler> logger) : IRequestHandler<RemoveRoomTypeCommand, Result>
{
    public async Task<Result> Handle(
        RemoveRoomTypeCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing room type {RoomTypeId}.", request.RoomTypeId);
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);

        if (roomType is null)
        {
            logger.LogWarning("Room type {RoomTypeId} was not found.",
                request.RoomTypeId);
            return Result.Failure(RoomTypeErrors.NotFound());
        }

        await hotelAccessService.EnsureCurrentUserManagesAsync(
            roomType.HotelId,
            cancellationToken);
        roomType.Delete();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Room type {RoomTypeId} was successfully removed from hotel {HotelId}.",
            roomType.Id,
            roomType.HotelId);
        return Result.Success();
    }
}