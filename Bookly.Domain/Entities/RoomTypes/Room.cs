using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.RoomTypes;

public class Room : BaseEntity
{
    public int RoomTypeId { get; private set; }
    public string RoomNumber { get; private set; } = null!;
    private Room() { }
    private Room(
        int roomTypeId,
        string roomNumber)
    {
        RoomTypeId = roomTypeId;
        RoomNumber = roomNumber;
    }
    public static Room Create(
       int roomTypeId,
       string roomNumber)
    {
        if (roomTypeId <= 0)
            throw new ArgumentException("Room type is required.");

        if (string.IsNullOrWhiteSpace(roomNumber))
            throw new ArgumentException("Room number is required.");

        return new Room(
            roomTypeId,
            roomNumber);
    }

}
