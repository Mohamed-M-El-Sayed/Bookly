using Bookly.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;
using Bookly.Application.Features.CancellationPolicies.Commands.UpdateCancellationRule;
using Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;
using Bookly.Application.Features.Hotels.Commands.CreateHotel;
using Bookly.Application.Features.Hotels.Commands.RemoveHotelAmenity;
using Bookly.Application.Features.Hotels.Commands.SetHotelAmenities;
using Bookly.Application.Features.Hotels.Commands.UpdateHotel;
using Bookly.Application.Features.Hotels.Queries;
using Bookly.Application.Features.Hotels.Queries.GetHotelAmenities;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Gets the active amenities assigned to a hotel.
    /// </summary>
    [HttpGet("{hotelId:int}/amenities")]
    public async Task<IActionResult> GetHotelAmenities([FromRoute] int hotelId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetHotelAmenitiesQuery(hotelId), cancellationToken);
        return Respond(result);
    }
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
    /// <summary>
    ///  Sets the amenities for a hotel (Hotel Manager only).
    /// </summary>
    [HttpPost("{id:int}/amenities")]
    public async Task<IActionResult> SetHotelAmenities(int id, [FromBody] SetHotelAmenitiesCommand command, CancellationToken cancellationToken)
    {
        command.HotelId = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Removes one amenity from a hotel (Hotel manager only).
    /// </summary>
    [HttpDelete("{hotelId:int}/amenities/{amenityId:int}")]
    public async Task<IActionResult> RemoveAmenity(
        int hotelId, int amenityId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveHotelAmenityCommand(hotelId, amenityId), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Retrieves all cancellation rules for a specific hotel.
    /// </summary>
    [HttpGet("{hotelId:int}/cancellation-policy/rules")]
    public async Task<IActionResult> GetCancellationRules(int hotelId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCancellationRulesQuery(hotelId), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Creates a cancellation policy for a hotel or adds missing cancellation rules to its existing policy (Hotel Manager only).
    /// </summary>
    [HttpPost("{hotelId:int}/cancellation-policy")]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> CreateCancellationPolicy(int hotelId, [FromBody] IReadOnlyCollection<CancellationRuleRequest> rules,
    CancellationToken cancellationToken)
    {
        var command = new CreateCancellationPolicyCommand()
        {
            HotelId = hotelId,
            Rules = rules
        };
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Updates an existing cancellation rule for a hotel (Hotel Manager only).
    /// </summary>
    [HttpPut("{hotelId:int}/cancellation-policy/rules/{ruleId:int}")]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> UpdateCancellationRule(int hotelId, int ruleId, [FromBody] UpdateCancellationRuleCommand command,
        CancellationToken cancellationToken)
    {
        command.HotelId = hotelId;
        command.RuleId = ruleId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}
