using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Availability;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;

namespace Bookly.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(IUnitOfWork unitOfWork,
    IRoomAvailabilityService roomAvailabilityService) : IRequestHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public async Task<Result<CreateRoomResponse>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>().GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType is null)
            return Result<CreateRoomResponse>.Failure(RoomTypeErrors.NotFound());

        var room = Room.Create(request.RoomTypeId, request.RoomNumber);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Repository<Room>()
                .AddAsync(room, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var syncResult = await roomAvailabilityService.SyncRoomTypeAsync(
                room.RoomTypeId,
                DateOnly.FromDateTime(DateTime.Today),
                cancellationToken);

            if (syncResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result<CreateRoomResponse>.Failure(syncResult.Error!);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var response = new CreateRoomResponse(
               room.Id,
               room.RoomTypeId,
               room.RoomNumber);
            return Result<CreateRoomResponse>.Success(response);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
