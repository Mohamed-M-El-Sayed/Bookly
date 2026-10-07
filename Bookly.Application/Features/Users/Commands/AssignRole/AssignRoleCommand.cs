using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Users.Commands.AssignRole;

public class AssignRoleCommand() : IRequest<Result>
{
    public string UserId { get; set; } = default!;
    public string Role { get; set; } = default!;
}
