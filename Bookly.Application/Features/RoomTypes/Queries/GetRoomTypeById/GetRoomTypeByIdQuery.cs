using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public record GetRoomTypeByIdQuery(int HotelId, int Id) : IRequest<Result<GetRoomTypeByIdResponse>>;
