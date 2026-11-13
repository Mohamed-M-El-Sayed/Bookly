using Bookly.Application.Features.HotelImages.Commands.AddHotelImage;
using Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/hotels/{hotelId}/images")]
[ApiController]
public class ImagesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Adds a new image to the specified hotel.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(int hotelId, [FromBody] AddHotelImageCommand command, CancellationToken cancellationToken)
    {
        command.HotelId = hotelId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Retrieves all images for the specified hotel.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(int hotelId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllHotelImagesQuery(hotelId), cancellationToken);
        return Respond(result);
    }
}
