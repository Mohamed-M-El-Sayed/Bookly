using Bookly.Application.Features.SeasonalPricing.Commands.CreateSeasonalPrice;
using Bookly.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPrice;
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
    /// Creates a seasonal price for a room type (Hotel manger only).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateSeasonalPrice(int roomTypeId, [FromBody] CreateSeasonalPriceCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = roomTypeId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result, StatusCodes.Status201Created);
    }

    public record CreateSeasonalPriceRequest(
        int RoomTypeId,
        string Name,
        DateOnly StartDate,
        DateOnly EndDate,
        decimal PricePerNight);
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
        return Respond(result, StatusCodes.Status204NoContent);
    }

}
