using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.RoomTypes.Commands.RemoveRoomTypeAmenity
{
    public class RemoveRoomTypeAmenityCommand(int roomTypeId, int amenityId) : IRequest<Result>, ICacheInvalidationRequest
    {
        [JsonIgnore]
        public int RoomTypeId { get; } = roomTypeId;
        public int AmenityId { get; } = amenityId;
        public IReadOnlyCollection<string> Tags => [CacheTags.RoomTypeAmenities];
    }
}
