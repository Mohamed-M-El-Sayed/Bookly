using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Bookings.Shared;

public record BookingResponse(int Id,
 string Reference,
 string GuestName,
 string HotelName,
 DateOnly CheckIn,
 DateOnly CheckOut,
 decimal TotalPrice,
 BookingStatus Status);
