using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Domain.Entities.Bookings;

public class BookingRoom : SoftDeletableEntity
{
    public int BookingId { get; private set; }
    public int RoomTypeId { get; private set; }
    public int? BoardOptionId { get; private set; }
    public int? RoomId { get; private set; }
    public int Guests { get; private set; }
    // total for this room for the whole stay, board included
    public decimal PriceAtBooking { get; private set; }
    public RoomType RoomType { get; private set; } = null!;
    private BookingRoom() { }

    private BookingRoom(
        int roomTypeId,
        int? boardOptionId,
        int guests,
        decimal priceAtBooking)
    {
        RoomTypeId = roomTypeId;
        BoardOptionId = boardOptionId;
        Guests = guests;
        PriceAtBooking = priceAtBooking;
    }
    public static BookingRoom Create(
        int roomTypeId,
        int? boardOptionId,
        int guests,
        decimal priceAtBooking)
    {
        if (roomTypeId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roomTypeId));
        if (guests <= 0)
            throw new ArgumentOutOfRangeException(nameof(guests));
        if (priceAtBooking < 0)
            throw new ArgumentOutOfRangeException(nameof(priceAtBooking));
        return new BookingRoom(
            roomTypeId,
            boardOptionId,
            guests,
            priceAtBooking);
    }

    public void AssignRoom(int roomId)
    {
        if (roomId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roomId));

        RoomId = roomId;
    }
}