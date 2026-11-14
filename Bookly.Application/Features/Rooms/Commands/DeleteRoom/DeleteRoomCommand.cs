using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(int RoomId) : IRequest<Result>;
