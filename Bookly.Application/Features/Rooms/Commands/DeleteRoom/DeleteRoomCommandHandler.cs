using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Availability;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Rooms.Errors;
using Bookly.Application.Features.Rooms.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork,
    IRoomAvailabilityService roomAvailabilityService,
    IHotelAccessService hotelAccessService,
    ILogger<DeleteRoomCommandHandler> logger) : IRequestHandler<DeleteRoomCommand, Result>
{
    public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting room {RoomId}", request.RoomId);
        var spec = new RoomWithRoomTypeSpecification(request.RoomId);

        var room = await unitOfWork.Repository<Room>()
            .GetEntityWithSpecAsync(spec, cancellationToken);

        if (room is null)
            return Result.Failure(RoomErrors.NotFound());

        await hotelAccessService.EnsureCurrentUserManagesAsync(
            room.RoomType.HotelId, cancellationToken);
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var roomTypeId = room.RoomTypeId;

            unitOfWork.Repository<Room>().Remove(room);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var syncResult = await roomAvailabilityService.SyncRoomTypeAsync(
                roomTypeId,
                DateOnly.FromDateTime(DateTime.Today),
                cancellationToken);

            if (syncResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                logger.LogWarning("Failed to synchronize availability when deleting room {RoomId}. Error: {Error}",
                    request.RoomId, syncResult.Error);
                return Result.Failure(syncResult.Error!);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            logger.LogInformation("Room {RoomId} deleted successfully",
                room.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            logger.LogError(
                ex,
                "Unexpected error occurred while deleting room {RoomId}",
                request.RoomId);
            throw;
        }
    }
}