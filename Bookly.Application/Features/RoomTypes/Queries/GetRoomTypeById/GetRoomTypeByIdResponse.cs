namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public record GetRoomTypeByIdResponse(
    int Id,
    int HotelId,
    string Name,
    decimal Price,
    string Description,
    string View);
