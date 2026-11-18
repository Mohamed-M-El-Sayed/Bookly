using Bookly.Application.Features.Users.Commands.AssignRole;
using Bookly.Application.Features.Users.Commands.UnAssignRole;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class UsersController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Assigns a role to a user (Admin only).
    /// </summary>
    [HttpPost("{userId}/roles")]
    public async Task<IActionResult> AssignRole(string userId, [FromBody] AssignRoleCommand command, CancellationToken cancellationToken)
    {
        command.UserId = userId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Removes a role from a user (Admin only).
    /// </summary>
    [HttpDelete("{userId}/roles")]
    public async Task<IActionResult> UnAssignRole(string userId, [FromBody] UnAssignRoleCommand command, CancellationToken cancellationToken)
    {
        command.UserId = userId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}
