using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand : IRequest<Result<CancelBookingResponse>>, ICacheInvalidationRequest
{
    [JsonIgnore]
    public int BookingId { get; set; }
    public string? Reason { get; set; }
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];
}
