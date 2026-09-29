using Bookly.Domain.Entities.Base;
using Bookly.Domain.Enums;

namespace Bookly.Domain.Entities.Bookings;

public class Booking : BaseEntity
{
    public int HotelId { get; private set; }
    public Guid UserId { get; private set; }
    public DateOnly CheckIn { get; private set; }
    public DateOnly CheckOut { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    private readonly List<BookingRoom> _bookingRooms = new();
    public IReadOnlyCollection<BookingRoom> BookingRooms => _bookingRooms.AsReadOnly();
    private Booking() { }


}
