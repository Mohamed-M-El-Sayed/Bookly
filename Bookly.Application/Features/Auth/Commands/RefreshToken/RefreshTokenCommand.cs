using Bookly.Application.Common.Results;
using Bookly.Application.Features.Auth.Shared;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<AuthResponse>>
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
