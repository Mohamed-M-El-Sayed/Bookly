using Bookly.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;
using Bookly.Application.Features.CancellationPolicies.Commands.DeleteCancellationRule;
using Bookly.Application.Features.CancellationPolicies.Commands.UpdateCancellationRule;
using Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRuleById;
using Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[ApiController]
[Route("api/hotels/{hotelId:int}/cancellation-policy")]
public class CancellationPoliciesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Retrieves all cancellation rules for a hotel.
    /// </summary>
    [HttpGet("rules")]
    public async Task<IActionResult> GetRules(int hotelId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCancellationRulesQuery(hotelId), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Retrieves a cancellation rule by ID.
    /// </summary>
    [HttpGet("rules/{ruleId:int}")]
    public async Task<IActionResult> GetRuleById(int hotelId, int ruleId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCancellationRuleByIdQuery(ruleId), cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Creates a cancellation policy or adds missing rules to the hotel's existing policy (Hotel Manager only).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Create(int hotelId, [FromBody] IReadOnlyCollection<CancellationRuleRequest> rules,
        CancellationToken cancellationToken)
    {
        var command = new CreateCancellationPolicyCommand
        {
            HotelId = hotelId,
            Rules = rules
        };
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Updates a cancellation rule (Hotel Manager only).
    /// </summary>
    [HttpPut("rules/{ruleId:int}")]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Update(int hotelId, int ruleId,
        [FromBody] UpdateCancellationRuleCommand command, CancellationToken cancellationToken)
    {
        command.HotelId = hotelId;
        command.RuleId = ruleId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Deletes a cancellation rule (Hotel Manager only).
    /// </summary>
    [HttpDelete("rules/{ruleId:int}")]
    [Authorize(Roles = Roles.Manager)]
    public async Task<IActionResult> Delete(int hotelId, int ruleId, CancellationToken cancellationToken)
    {
        var command = new DeleteCancellationRuleCommand
        {
            HotelId = hotelId,
            RuleId = ruleId
        };
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}
