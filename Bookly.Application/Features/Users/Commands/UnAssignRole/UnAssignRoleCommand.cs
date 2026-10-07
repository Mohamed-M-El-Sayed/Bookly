using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Users.Commands.UnAssignRole;

public class UnAssignRoleCommand() : IRequest<Result>
{
    public string UserId { get; set; } = default!;
    public string Role { get; set; } = default!;
}
