using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.ForgetPassword;

public class ForgetPasswordCommand : IRequest<Result<ForgetPasswordResponse>>
{
    public string Email { get; set; } = string.Empty;
}
