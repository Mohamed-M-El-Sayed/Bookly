using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Rooms.Errors;

public static class RoomErrors
{
    public static Error CannotReduceInventory(DateOnly date) =>
        ErrorFactory.Conflict("availability.inventory.conflict",
            $"Cannot reduce room inventory because there are existing reservations on {date}");
    public static Error NotFound() =>
        ErrorFactory.NotFound("room.notfound", "Room not found");
    public static Error RoomNumberAlreadyExists() =>
        ErrorFactory.Conflict("Room.RoomNumberAlreadyExists",
        "A room with this number already exists in this room type.");
}
