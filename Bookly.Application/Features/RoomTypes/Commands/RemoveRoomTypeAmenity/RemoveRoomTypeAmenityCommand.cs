using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.RoomTypes.Commands.RemoveRoomTypeAmenity
{
    public class RemoveRoomTypeAmenityCommand(int roomTypeId, int amenityId) : IRequest<Result>
    {
        [JsonIgnore]
        public int RoomTypeId { get; } = roomTypeId;
        public int AmenityId { get; } = amenityId;
    }
}
