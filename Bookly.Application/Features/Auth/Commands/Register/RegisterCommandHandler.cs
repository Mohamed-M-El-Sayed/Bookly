using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Contracts.Services.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(
    IAuthService authService,
    ILogger<RegisterCommandHandler> logger,
    ICompromisedPasswordService compromisedPasswordService)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Registration attempt started for email {Email}",
            request.Email);

        var isCompromised =
            await compromisedPasswordService.IsCompromisedPasswordAsync(
                request.Password,
                cancellationToken);

        if (isCompromised)
        {
            logger.LogWarning(
                "Registration failed for {Email}. Password is compromised.",
                request.Email);

            return Result<RegisterResponse>.Failure(
                Errors.Validation(
                    "The provided password is compromised."));
        }

        var result = await authService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName.Trim(),
            request.LastName.Trim(),
            cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Registration failed for {Email}. Code: {ErrorCode}, Reason: {ErrorMessage}",
                request.Email,
                result.Error!.Code,
                result.Error.Message);

            return Result<RegisterResponse>.Failure(result.Error);
        }

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                "Registration successful. Please check your email to verify your account."));
    }
}