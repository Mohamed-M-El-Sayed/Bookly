using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.ResendConfirmation;

public class ResendConfirmationCommandHandler(IAuthService authService , 
    ILogger<ResendConfirmationCommandHandler> logger) : IRequestHandler<ResendConfirmationCommand, Result<ResendConfirmationResponse>>
{
    public async Task<Result<ResendConfirmationResponse>> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ResendConfrimationEmailAsync(request.Email, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogWarning(
                "Resend confirmation failed. Code: {ErrorCode}, Reason: {ErrorMessage}",
                result.Error!.Code, result.Error.Message);

            return Result<ResendConfirmationResponse>.Failure(result.Error);
        }
        logger.LogInformation("Resend confirmation request processed");
        return Result<ResendConfirmationResponse>.Success(
                new ResendConfirmationResponse(
                    "If the account exists and is not confirmed yet, a new confirmation email has been sent."));
        
    }
}
