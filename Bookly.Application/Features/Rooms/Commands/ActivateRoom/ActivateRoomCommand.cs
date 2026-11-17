using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Rooms.Commands.ActivateRoom;

public record ActivateRoomCommand(int RoomId) : IRequest<Result>;
