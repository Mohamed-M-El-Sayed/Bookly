using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.ForgetPassword;

public class ForgetPasswordCommand : IRequest<Result<MessageResponse>>
{
    public string Email { get; set; } = string.Empty;
}
