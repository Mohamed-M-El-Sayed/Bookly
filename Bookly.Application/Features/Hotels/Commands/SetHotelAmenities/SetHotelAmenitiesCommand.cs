using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Hotels.Commands.SetHotelAmenities;

public class SetHotelAmenitiesCommand : IRequest<Result>
{
    [JsonIgnore]
    public int HotelId { get; set; }
    public List<int> AmenityIds { get; set; } = new();
}
