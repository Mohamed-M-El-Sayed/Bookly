using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Users.Commands.UnAssignRole;

public class UnAssignRoleCommandHandler(
    IUserService userService,
    ILogger<UnAssignRoleCommandHandler> logger)
    : IRequestHandler<UnAssignRoleCommand, Result>
{
    public async Task<Result> Handle(
        UnAssignRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Removing role {Role} from user {UserId}",
            request.Role, request.UserId);

        var result = await userService.UnAssignRoleAsync(
            request.UserId, request.Role, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Failed to remove role {Role} from user {UserId}. Code: {ErrorCode}, Reason: {ErrorMessage}",
                request.Role, request.UserId, result.Error!.Code, result.Error.Message);

            return result;
        }

        logger.LogInformation(
            "Role {Role} removed from user {UserId}",
            request.Role, request.UserId);

        return result;
    }
}
