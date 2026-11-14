using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.RoomTypes;

public class Room : BaseEntity
{
    public int RoomTypeId { get; private set; }
    public string RoomNumber { get; private set; } = null!;
    public bool IsActive { get; set; }
    public RoomType RoomType { get; private set; } = null!;
    private Room() { }
    private Room(
        int roomTypeId,
        string roomNumber)
    {
        RoomTypeId = roomTypeId;
        RoomNumber = roomNumber;
        IsActive = true;
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
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
