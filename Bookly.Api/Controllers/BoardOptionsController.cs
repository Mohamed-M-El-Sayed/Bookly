using Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;
using Bookly.Application.Features.BoardOptions.Commands.DeleteBoardOption;
using Bookly.Application.Features.BoardOptions.Commands.UpdateBoardOption;
using Bookly.Application.Features.BoardOptions.Queries.GetBoardOptionById;
using Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[ApiController]
[Route("api/room-types/{roomTypeId}/board-options")]
[Authorize(Roles = Roles.Manager)]
public class BoardOptionsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Adds a new board option (e.g. breakfast, half board) to the specified room type (Hotel manager only).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(int roomTypeId, [FromBody] CreateBoardOptionCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Updates a board option for the specified room type (Hotel manager only).
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int roomTypeId, int id, [FromBody] UpdateBoardOptionCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = roomTypeId;
        command.BoardOptionId = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Deletes a board option (Hotel manger only).
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBoardOption(int roomTypeId, int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteBoardOptionCommand(id, roomTypeId), cancellationToken);
        return Respond(result, StatusCodes.Status204NoContent);
    }
    /// <summary>
    /// Gets a board option by ID (Hotel manger only).
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBoardOptionById(int roomTypeId, int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBoardOptionByIdQuery(id, roomTypeId), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Gets board options for a room type (Hotel manger only).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRoomTypeBoardOptions(int roomTypeId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetRoomTypeBoardOptionsQuery(roomTypeId), cancellationToken);

        return Respond(result);
    }


}
