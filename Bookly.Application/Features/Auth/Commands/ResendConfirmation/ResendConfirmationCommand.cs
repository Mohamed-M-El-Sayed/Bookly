using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Auth.Commands.ResendConfirmation;

public class ResendConfirmationCommand : IRequest<Result<ResendConfirmationResponse>>
{
    public string Email { get; set; } = string.Empty; 
}
