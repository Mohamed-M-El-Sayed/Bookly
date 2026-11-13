using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Rooms.Commands.DeactivateRoom;

public record DeactivateRoomCommand(int RoomId) : IRequest<Result>;
