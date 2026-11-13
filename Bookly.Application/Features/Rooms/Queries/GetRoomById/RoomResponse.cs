namespace Bookly.Application.Features.Rooms.Queries.GetRoomById;

public record RoomResponse(int Id, string RoomNumber, int RoomTypeId, bool IsActive);
