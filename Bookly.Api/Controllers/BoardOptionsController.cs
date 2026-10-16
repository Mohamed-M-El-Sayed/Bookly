using Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[ApiController]
[Route("api/room-types/{roomTypeId}/board-options")]
public class BoardOptionsController(IMediator mediator) : ApiControllerBase
{
    // admin 
    /// <summary>
    /// Adds a new board option (e.g. breakfast, half board) to the specified room type.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(int roomTypeId, [FromBody] CreateBoardOptionCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }



}
