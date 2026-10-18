using Bookly.Application.Features.Amenities.Commands.CreateAmenity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenitiesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new amenity (Admin only).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAmenityCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}
