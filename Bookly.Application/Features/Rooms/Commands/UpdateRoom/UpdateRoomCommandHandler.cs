using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Rooms.Errors;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<UpdateRoomCommandHandler> logger)
    : IRequestHandler<UpdateRoomCommand, Result>
{
    public async Task<Result> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);

        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result.Failure(accessResult.Error!);

        var room = await unitOfWork.Repository<Room>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (room is null)
        {
            logger.LogWarning(
                "Room {RoomId} was not found",
                request.Id);

            return Result.Failure(RoomErrors.NotFound());
        }
        if (room.RoomTypeId != request.RoomTypeId)
            return Result.Failure(RoomErrors.NotFound());

        var roomNumberExists = await unitOfWork.Repository<Room>()
            .AnyAsync(
                x => x.RoomTypeId == request.RoomTypeId &&
                     x.RoomNumber == request.RoomNumber &&
                     x.Id != room.Id,
                cancellationToken);
        if (roomNumberExists)
            return Result.Failure(RoomErrors.RoomNumberAlreadyExists());

        room.Update(request.RoomNumber);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Room {RoomId} was updated successfully",
            request.Id);
        return Result.Success();
    }
}