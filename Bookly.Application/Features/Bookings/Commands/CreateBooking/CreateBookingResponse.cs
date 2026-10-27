using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingResponse(
    string BookingReference,
    BookingStatus Status,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Nights,
    decimal TotalPrice,
    DateTime? ExpiresAt
);