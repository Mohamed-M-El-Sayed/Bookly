using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler(
    IAuthService authService,
    ILogger<ConfirmEmailCommandHandler> logger)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ConfirmEmailCommand for user {UserId}", request.UserId);

        var result = await authService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning("Confirm email failed for {UserId}. Code: {ErrorCode}, Reason: {ErrorMessage}",
                request.UserId, result.Error!.Code, result.Error.Message);
            return result;
        }

        logger.LogInformation("Email confirmed for user {UserId}", request.UserId);
        return Result.Success();
    }
}
