using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Availability;
using Bookly.Application.Features.Rooms.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Rooms.Commands.DeactivateRoom;

public class DeactivateRoomCommandHandler(IUnitOfWork unitOfWork,
    IRoomAvailabilityService roomAvailabilityService,
    ILogger<DeactivateRoomCommandHandler> logger) : IRequestHandler<DeactivateRoomCommand, Result>
{
    public async Task<Result> Handle(DeactivateRoomCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deactivating room {RoomId}.",
            request.RoomId);

        var room = await unitOfWork.Repository<Room>()
            .GetByIdAsync(request.RoomId, cancellationToken);

        if (room is null)
        {
            return Result.Failure(RoomErrors.NotFound());
        }

        if (!room.IsActive)
        {
            logger.LogInformation(
                "Room {RoomId} is already inactive.",
                request.RoomId);

            return Result.Success();
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            room.Deactivate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var syncResult = await roomAvailabilityService.SyncRoomTypeAsync(
                room.RoomTypeId,
                DateOnly.FromDateTime(DateTime.Today),
                cancellationToken);

            if (syncResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);

                logger.LogWarning(
                    "Failed to synchronize availability when deactivating room {RoomId}. Error: {Error}",
                    request.RoomId,
                    syncResult.Error);

                return Result.Failure(syncResult.Error!);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation(
                "Room {RoomId} was successfully deactivated.",
                request.RoomId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            logger.LogError(
                ex,
                "An error occurred while deactivating room {RoomId}.",
                request.RoomId);

            throw;
        }
    }
}