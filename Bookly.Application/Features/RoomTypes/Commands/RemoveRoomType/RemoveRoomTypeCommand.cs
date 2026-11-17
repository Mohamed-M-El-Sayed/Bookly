using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Commands.RemoveRoomType;

public record RemoveRoomTypeCommand(int RoomTypeId) : IRequest<Result>;
