using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
