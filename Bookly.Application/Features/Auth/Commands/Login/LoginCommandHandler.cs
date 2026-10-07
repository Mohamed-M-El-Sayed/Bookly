using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Features.Auth.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(IAuthService authService , ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
       logger.LogInformation("Handling LoginCommand for email: {Email}", request.Email);
       var result = await authService.LoginAsync(request.Email, request.Password, cancellationToken);
       return result;   
    }
}
