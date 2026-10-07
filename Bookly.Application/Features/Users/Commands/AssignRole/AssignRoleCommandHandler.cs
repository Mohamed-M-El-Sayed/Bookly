using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Users.Commands.AssignRole;

public class AssignRoleCommandHandler(
    IUserService userService,
    ILogger<AssignRoleCommandHandler> logger)
    : IRequestHandler<AssignRoleCommand, Result>
{
    public async Task<Result> Handle(
        AssignRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Assigning role {Role} to user {UserId}",
            request.Role, request.UserId);

        var result = await userService.AssignRoleAsync(
            request.UserId, request.Role, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Failed to assign role {Role} to user {UserId}. Code: {ErrorCode}, Reason: {ErrorMessage}",
                request.Role, request.UserId, result.Error!.Code, result.Error.Message);

            return result;
        }
        logger.LogInformation(
            "Role {Role} assigned to user {UserId}",
            request.Role, request.UserId);

        return result;
    }
}