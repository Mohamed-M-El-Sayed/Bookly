using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Users.Errors;

public static class UserErrors
{
    public static Error UserNotFound() =>
       Application.Common.Results.ErrorFactory.NotFound("User.NotFound", "User not found.");
    public static Error RoleNotFound() =>
       Application.Common.Results.ErrorFactory.NotFound("Role.NotFound", "Role not found.");
}
