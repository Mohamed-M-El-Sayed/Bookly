using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Features.Auth.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IAuthService authService,
    ILogger<RefreshTokenCommandHandler> logger)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Refresh token request started");

        var result = await authService.RefreshTokenAsync(
            request.AccessToken, request.RefreshToken, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Refresh token failed. Code: {ErrorCode}, Reason: {ErrorMessage}",
                result.Error!.Code, result.Error.Message);

            return result;
        }

        logger.LogInformation("Tokens refreshed for user {UserId}", result.Value!.User.Id);

        return result;
    }
}