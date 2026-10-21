using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.RoomTypes.Commands.SetRoomTypesAmenities;

public class SetRoomTypesAmenitiesCommand : IRequest<Result>
{
    [JsonIgnore]
    public int RoomTypeId { get; set; }
    public List<int> AmenityIds { get; set; } = new();
}
