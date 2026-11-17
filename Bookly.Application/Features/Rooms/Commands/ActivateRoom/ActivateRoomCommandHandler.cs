using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Availability;
using Bookly.Application.Features.Rooms.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Rooms.Commands.ActivateRoom;

public class ActivateRoomCommandHandler(IUnitOfWork unitOfWork,
    IRoomAvailabilityService roomAvailabilityService,
    ILogger<ActivateRoomCommandHandler> logger) : IRequestHandler<ActivateRoomCommand, Result>
{
    public async Task<Result> Handle(ActivateRoomCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Activating room {RoomId}.", request.RoomId);
        var room = await unitOfWork.Repository<Room>()
            .GetByIdAsync(request.RoomId, cancellationToken);

        if (room is null)
            return Result.Failure(RoomErrors.NotFound());

        if (room.IsActive)
            return Result.Success();

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            room.Activate();
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var syncResult = await roomAvailabilityService.SyncRoomTypeAsync(
                room.RoomTypeId,
                DateOnly.FromDateTime(DateTime.Today),
                cancellationToken);

            if (syncResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                logger.LogWarning(
                   "Failed to synchronize availability when activating room {RoomId}. Error: {Error}",
                   request.RoomId, syncResult.Error);
                return Result.Failure(syncResult.Error!);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            logger.LogInformation(
                "Room {RoomId} was successfully activated.", request.RoomId);

            return Result.Success();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}