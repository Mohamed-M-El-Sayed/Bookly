namespace Bookly.Application.Features.Auth.Shared;

public record UserResponse(
    Guid Id,
    string Email,
    IList<string> Roles);
