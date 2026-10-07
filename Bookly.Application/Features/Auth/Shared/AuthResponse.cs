namespace Bookly.Application.Features.Auth.Shared;

public record class AuthResponse(
    UserResponse User,
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken ,
    DateTime RefreshTokenExpiresAt );


