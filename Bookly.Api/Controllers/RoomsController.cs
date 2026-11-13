using Bookly.Application.Features.Rooms.Commands.ActivateRoom;
using Bookly.Application.Features.Rooms.Commands.CreateRoom;
using Bookly.Application.Features.Rooms.Commands.DeactivateRoom;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/room-types/{roomTypeId}/rooms")]
[ApiController]
[Authorize(Roles = Roles.Manager)]
public class RoomsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new room for the specified room type.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(int roomTypeId, [FromBody] CreateRoomCommand command, CancellationToken cancellation)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellation);
        return Respond(result);
    }
    /// <summary>
    /// Activates a room (Hotel manager only).
    /// </summary>
    [HttpPatch("{roomId:int}/activate")]
    public async Task<IActionResult> ActivateRoom(
        int roomId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivateRoomCommand(roomId),
            cancellationToken);

        return Respond(result);
    }

    /// <summary>
    /// Deactivates a room (Hotel manager only).
    /// </summary>
    [HttpPatch("{roomId:int}/deactivate")]
    public async Task<IActionResult> DeactivateRoom(int roomId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeactivateRoomCommand(roomId),
            cancellationToken);

        return Respond(result);
    }
}
