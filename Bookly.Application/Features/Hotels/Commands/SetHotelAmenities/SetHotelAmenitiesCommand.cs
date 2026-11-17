using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Hotels.Commands.SetHotelAmenities;

public class SetHotelAmenitiesCommand : IRequest<Result>, ICacheInvalidationRequest
{
    [JsonIgnore]
    public int HotelId { get; set; }
    public List<int> AmenityIds { get; set; } = new();

    public IReadOnlyCollection<string> Tags => [CacheTags.HotelAmenities];
}
