using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotelAmenity;

public class RemoveHotelAmenityCommand(int hotelId, int amenityId) : IRequest<Result>
{
    [JsonIgnore]
    public int HotelId { get; } = hotelId;
    public int AmenityId { get; } = amenityId;
}
