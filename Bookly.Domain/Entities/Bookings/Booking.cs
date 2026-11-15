using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.Payments;
using Bookly.Domain.Enums;
using Bookly.Domain.Exceptions;

namespace Bookly.Domain.Entities.Bookings;

public class Booking : SoftDeletableEntity
{
    private static readonly TimeSpan HoldDuration = TimeSpan.FromMinutes(15);
    public string Reference { get; private set; } = default!;
    public int HotelId { get; private set; }
    public Guid UserId { get; private set; }
    public string GuestName { get; private set; } = default!;
    public DateOnly CheckIn { get; private set; }
    public DateOnly CheckOut { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalPrice => _bookingRooms.Sum(x => x.PriceAtBooking);
    public string Email { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public DateTime? ExpiresAt { get; private set; }
    private readonly List<BookingRoom> _bookingRooms = new();
    public IReadOnlyCollection<BookingRoom> BookingRooms => _bookingRooms.AsReadOnly();
    public Hotel Hotel { get; private set; } = default!;
    private readonly List<Payment> _payments = [];
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
    private Booking() { }
    public static Booking Create(
        Guid userId,
        int hotelId,
        DateOnly checkIn,
        DateOnly checkOut,
        string guestName,
        string email,
        string phoneNumber)
    {
        return new Booking
        {
            Reference = GenerateReference(),
            UserId = userId,
            HotelId = hotelId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestName = guestName.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            Email = email.Trim(),
            Status = BookingStatus.Pending,
            ExpiresAt = DateTime.UtcNow.Add(HoldDuration)
        };
    }
    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException("Only pending bookings can be confirmed.");
        if (ExpiresAt <= DateTime.UtcNow)
            throw new DomainException("Expired bookings cannot be confirmed.");
        Status = BookingStatus.Confirmed;
    }
    private static string GenerateReference()
    {
        return $"BK-{Guid.NewGuid():N}"[..11].ToUpperInvariant();
    }
    public void AddRoom(
    int roomTypeId,
    int? boardOptionId,
    int guests,
    decimal priceAtBooking)
    {
        var bookingRoom = BookingRoom.Create(
        roomTypeId,
        boardOptionId,
        guests,
        priceAtBooking);

        _bookingRooms.Add(bookingRoom);
    }
    public bool TryExpire(DateTime utcNow)
    {
        if (Status != BookingStatus.Pending)
            return false;
        if (ExpiresAt is null || ExpiresAt.Value > utcNow)
            return false;
        Status = BookingStatus.Expired;
        return true;
    }
}
