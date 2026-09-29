using Bookly.Domain.Entities.Base;
using Bookly.Domain.Exceptions;

namespace Bookly.Domain.Entities.RoomTypes;

public class RoomAvailability : BaseEntity
{
    public int RoomTypeId { get; private set; }
    public DateOnly Date { get; private set; }
    public int TotalRooms { get; private set; }
    public int AvailableRooms { get; private set; }
    public int ReservedRooms { get; private set; }
    public RoomType RoomType { get; private set; } = null!;
    public RoomAvailability() { }
    public RoomAvailability(int roomTypeId, DateOnly date, int totalRooms)
    {
        if (roomTypeId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roomTypeId));

        if (totalRooms < 0)
            throw new ArgumentOutOfRangeException(nameof(totalRooms));

        RoomTypeId = roomTypeId;
        Date = date;
        TotalRooms = totalRooms;
        AvailableRooms = totalRooms;
        ReservedRooms = 0;
    }

    public void Reserve()
    {
        if (AvailableRooms <= 0)
            throw new DomainException($"No available rooms for room type {RoomTypeId} on {Date:yyyy-MM-dd}.");

        AvailableRooms--;
        ReservedRooms++;
    }
    public void Release()
    {
        if (ReservedRooms <= 0)
            throw new DomainException($"No reserved rooms to release for room type {RoomTypeId} on {Date:yyyy-MM-dd}.");

        AvailableRooms++;
        ReservedRooms--;
    }

}
