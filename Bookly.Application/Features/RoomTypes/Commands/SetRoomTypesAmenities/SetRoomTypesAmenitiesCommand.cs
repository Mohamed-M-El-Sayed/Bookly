using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.RoomTypes.Commands.SetRoomTypesAmenities;

public class SetRoomTypesAmenitiesCommand : IRequest<Result>, ICacheInvalidationRequest
{
    [JsonIgnore]
    public int RoomTypeId { get; set; }
    public List<int> AmenityIds { get; set; } = new();
    public IReadOnlyCollection<string> Tags => [CacheTags.RoomTypeAmenities];
}
