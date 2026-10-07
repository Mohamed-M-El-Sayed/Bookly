namespace Bookly.Application.Contracts.Services.Authentication;

public record RefreshTokenn(string Token, DateTime ExpiresAt);
