using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Features.Rooms.Queries.GetRoomById;
using MediatR;

namespace Bookly.Application.Features.Rooms.Queries.GetRoomTypeRooms;

public record GetRoomTypeRoomsQuery(int RoomTypeId, int PageNumber, int PageSize)
    : IRequest<Result<PagedResult<RoomResponse>>>;
