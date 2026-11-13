namespace Bookly.Domain.Entities.RoomTypes;

public class RoomAvailability
{
    public int RoomTypeId { get; private set; }
    public DateOnly Date { get; private set; }
    public int TotalRooms { get; private set; }
    public int AvailableRooms => TotalRooms - ReservedRooms;
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
        ReservedRooms = 0;
    }

    public bool TryReserve()
    {
        if (AvailableRooms < 1)
            return false;
        ReservedRooms++;
        return true;
    }
    public bool TryRelease()
    {
        if (ReservedRooms <= 0)
            return false;
        ReservedRooms--;
        return true;
    }
    public bool TrySetTotalRooms(int totalRooms)
    {
        if (totalRooms < 0 || totalRooms < ReservedRooms)
            return false;

        TotalRooms = totalRooms;
        return true;
    }

}
