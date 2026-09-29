using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommand : IRequest<Result<CreateRoomResponse>>
{
    public int RoomTypeId { get; set; }
    public string RoomNumber { get; set; } = null!;
}
