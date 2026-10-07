using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
