using Bookly.Application.Common.Results;
using Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Queries.GetHotelRoomTypes;


public record GetHotelRoomTypesQuery(int HotelId) : IRequest<Result<IReadOnlyList<RoomTypeResponse>>>;

