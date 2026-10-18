using Bookly.Application.Features.Amenities.Commands.CreateAmenity;
using Bookly.Application.Features.Amenities.Queries.GetAmenities;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenitiesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Gets amenities by category ID.
    /// </summary>
    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory(int categoryId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAmenitiesQuery(categoryId), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Creates a new amenity (Admin only).
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAmenityCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

}
