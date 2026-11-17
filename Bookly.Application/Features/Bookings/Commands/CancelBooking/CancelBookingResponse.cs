using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingResponse(
 int BookingId,
 decimal TotalAmount,
 decimal CancellationFee,
 decimal RefundAmount,
 RefundStatus RefundStatus);
