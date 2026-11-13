using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Rooms.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;

namespace Bookly.Application.Features.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoomByIdQuery, Result<RoomResponse>>
{
    public async Task<Result<RoomResponse>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await unitOfWork.Repository<Room>().GetByIdAsync(request.Id);
        if (room is null)
            return Result<RoomResponse>.Failure(RoomErrors.NotFound());
        var response = room.Adapt<RoomResponse>();
        return Result<RoomResponse>.Success(response);
    }
}
