using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommand : IRequest<Result>
{
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int RoomTypeId { get; set; }
    public string RoomNumber { get; set; } = null!;
}
