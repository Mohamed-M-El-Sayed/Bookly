using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommand : IRequest<Result>
{
    public int BookingId { get; set; }
    public string? Reason { get; set; }
}
