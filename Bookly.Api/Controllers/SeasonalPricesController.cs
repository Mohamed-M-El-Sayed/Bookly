using Bookly.Application.Features.SeasonalPricing.Commands.CreateSeasonalPrice;
using Bookly.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPrice;
using Bookly.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPrice;
using Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPriceById;
using Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPricesByRoomTypeId;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/room-types/{roomTypeId:int}/seasonal-prices")]
[ApiController]
[Authorize(Roles = Roles.Manager)]
public class SeasonalPricesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Gets paginated seasonal prices for a room type (Hotel manger only).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSeasonalPrices(int roomTypeId, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 30, CancellationToken cancellationToken = default)
    {
        var query = new GetSeasonalPricesByRoomTypeIdQuery(
            roomTypeId,
            pageNumber,
            pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Gets a seasonal price by ID (Hotel manger only).
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSeasonalPriceById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSeasonalPriceByIdQuery(id), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Creates a seasonal price for a room type (Hotel manger only).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateSeasonalPrice(int roomTypeId, [FromBody] CreateSeasonalPriceCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result, StatusCodes.Status201Created);
    }

    /// <summary>
    /// Updates a seasonal price (Hotel manager only).
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSeasonalPrice(int roomTypeId, int id, [FromBody] UpdateSeasonalPriceCommand command,
        CancellationToken cancellationToken)
    {

        command.RoomTypeId = roomTypeId;
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Deletes a seasonal price (Hotel manger only).
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSeasonalPrice(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSeasonalPriceCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}
