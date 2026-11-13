using Bookly.Application.Features.Images.Commands.AddRoomTypeImage;
using Bookly.Application.Features.Images.Commands.DeleteRoomTypeImage;
using Bookly.Application.Features.Images.Queries.GetAllRoomTypeImages;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/roomtypes/{roomTypeId}/images")]
[ApiController]
public class RoomTypeImagesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Adds a new image to the specified room type.
    /// </summary>
    [Authorize(Roles = Roles.Manager)]
    [HttpPost]
    public async Task<IActionResult> Create(int roomTypeId, [FromBody] AddRoomTypeImageCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Retrieves all images for the specified room type.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(int roomTypeId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllRoomTypeImagesQuery(roomTypeId), cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Deletes a room type image.
    /// </summary>
    [Authorize(Roles = Roles.Manager)]
    [HttpDelete]
    public async Task<IActionResult> DeleteRoomTypeImage(int roomTypeId, int imageId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteRoomTypeImageCommand(roomTypeId, imageId),
            cancellationToken);
        return Respond(result, StatusCodes.Status204NoContent);
    }
}
