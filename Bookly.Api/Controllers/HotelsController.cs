using Bookly.Application.Features.Hotels.Commands.CreateHotel;
using Bookly.Application.Features.Hotels.Commands.UpdateHotel;
using Bookly.Application.Features.Hotels.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new hotel.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Updates an existing hotel.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHotelCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Searches for hotels based on the provided criteria.
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchHotelsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }


}
