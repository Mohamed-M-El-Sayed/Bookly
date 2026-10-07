using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Contracts.Services.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    IAuthService authService,
    ICompromisedPasswordService compromisedPasswordService,
    ILogger<ResetPasswordCommandHandler> logger)
    : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Password reset started for user {UserId}",
            request.UserId);

        var isCompromised =
            await compromisedPasswordService.IsCompromisedPasswordAsync(
                request.NewPassword,
                cancellationToken);

        if (isCompromised)
        {
            logger.LogWarning(
                "Password reset failed for user {UserId}. New password is compromised.",
                request.UserId);

            return Result.Failure(
                Errors.Validation(
                    "The provided password is compromised."));
        }

        var result = await authService.ResetPasswordAsync(
            request.UserId,
            request.Token,
            request.NewPassword,
            cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Password reset failed for user {UserId}. Code: {ErrorCode}, Reason: {ErrorMessage}",
                request.UserId,
                result.Error!.Code,
                result.Error.Message);

            return result;
        }

        logger.LogInformation(
            "Password reset completed for user {UserId}",
            request.UserId);

        return result;
    }
}
