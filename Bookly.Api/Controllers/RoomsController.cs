using Bookly.Application.Features.Rooms.Commands.ActivateRoom;
using Bookly.Application.Features.Rooms.Commands.CreateRoom;
using Bookly.Application.Features.Rooms.Commands.DeactivateRoom;
using Bookly.Application.Features.Rooms.Commands.DeleteRoom;
using Bookly.Application.Features.Rooms.Commands.UpdateRoom;
using Bookly.Application.Features.Rooms.Queries.GetRoomById;
using Bookly.Application.Features.Rooms.Queries.GetRoomTypeRooms;
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
    /// Gets a room by ID (Hotel manager only).
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int roomTypeId, int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRoomByIdQuery(id, roomTypeId), cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Gets all rooms belonging to a room type (Hotel manger only).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRoomTypeRooms(int roomTypeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetRoomTypeRoomsQuery(roomTypeId, pageNumber, pageSize),
            cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Creates a new room for the specified room type (Hotel manager only).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(int roomTypeId, [FromBody] CreateRoomCommand command, CancellationToken cancellation)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellation);
        return Respond(result);
    }
    /// <summary>
    /// Updates a room's number (Hotel manager only).
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRoom(int id, int roomTypeId, [FromBody] UpdateRoomCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Deletes a room (Hotel manager only).
    /// </summary>
    [HttpDelete("{roomId:int}")]
    public async Task<IActionResult> DeleteRoom(int roomId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteRoomCommand(roomId), cancellationToken);
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
