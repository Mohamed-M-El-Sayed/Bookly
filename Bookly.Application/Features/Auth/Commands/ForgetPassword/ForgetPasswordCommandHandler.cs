using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Features.Auth.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.ForgetPassword;

public class ForgetPasswordCommandHandler(
    IAuthService authService,
    ILogger<ForgetPasswordCommandHandler> logger)
    : IRequestHandler<ForgetPasswordCommand, Result<MessageResponse>>
{
    public async Task<Result<MessageResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ForgetPasswordCommand for email: {Email}", request.Email);
        var result = await authService.ForgetPasswordAsync(request.Email, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogWarning("Forget password failed for {Email}. Code: {ErrorCode}, Reason: {ErrorMessage}",
                request.Email, result.Error!.Code, result.Error.Message);
            return Result<MessageResponse>.Failure(result.Error);
        }

        return Result<MessageResponse>.Success(AuthMessages.ForgetPasswordResponse());

    }

}