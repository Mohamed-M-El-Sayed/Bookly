using Bookly.Api.Extensions;
using Bookly.Application.Features.Rooms.Commands.CreateRoom;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/room-types/{roomTypeId}/rooms")]
[ApiController]
public class RoomsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new room for the specified room type.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(int roomTypeId, [FromBody] CreateRoomCommand command, CancellationToken cancellation)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellation);
        return result.ToResponse();
    }
}
