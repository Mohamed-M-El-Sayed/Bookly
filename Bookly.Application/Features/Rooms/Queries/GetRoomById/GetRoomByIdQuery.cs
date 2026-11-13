using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Rooms.Queries.GetRoomById;

public record GetRoomByIdQuery(int Id, int RoomTypeId) : IRequest<Result<RoomResponse>>;

