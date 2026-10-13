using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;

namespace Bookly.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public async Task<Result<CreateRoomResponse>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>().GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType is null)
            return Result<CreateRoomResponse>.Failure(RoomTypeErrors.NotFound());

        var room = Room.Create(request.RoomTypeId, request.RoomNumber);

        await unitOfWork.Repository<Room>().AddAsync(room, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateRoomResponse(room.Id, room.RoomTypeId, room.RoomNumber);
        return Result<CreateRoomResponse>.Success(response);
    }
}
