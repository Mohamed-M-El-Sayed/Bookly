using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand : IRequest<Result<CancelBookingResponse>>
{
    [JsonIgnore]
    public int BookingId { get; set; }
    public string? Reason { get; set; }
}
