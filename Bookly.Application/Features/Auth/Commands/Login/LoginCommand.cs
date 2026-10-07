using Bookly.Application.Common.Results;
using Bookly.Application.Features.Auth.Shared;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<Result<AuthResponse>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
