using Bookly.Application.Features.Amenities.Commands.CreateAmenity;
using Bookly.Application.Features.Amenities.Commands.DeleteAmenity;
using Bookly.Application.Features.Amenities.Queries.GetAmenities;
using Bookly.Application.Features.Amenities.Queries.GetAmenityById;
using MediatR;
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
    /// Get amenity by Id (Admin only).
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAmenityByIdQuery(id), cancellationToken);
        return Respond(result);
    }
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
    /// <summary>
    /// Deletes an amenity that is not used by any hotel or room type (Admin only).
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return Respond(await mediator.Send(new DeleteAmenityCommand(id), cancellationToken));
    }


}
