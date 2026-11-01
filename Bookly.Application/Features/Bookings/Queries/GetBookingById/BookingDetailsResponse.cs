using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Bookings.Queries.GetBookingById;

public record BookingDetailsResponse(
    int Id,
    string Reference,
    int HotelId,
    string HotelName,
    string? HotelImageUrl,
    string GuestName,
    string PhoneNumber,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Nights,
    BookingStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt,
    BookingRoomResponse Room
);
public sealed record BookingRoomResponse(
    int RoomTypeId,
    string RoomTypeName,
    int? BoardOptionId,
    string? BoardOptionName,
    int Guests,
    decimal PriceAtBooking
);