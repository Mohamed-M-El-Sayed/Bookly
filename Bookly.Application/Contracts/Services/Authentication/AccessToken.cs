namespace Bookly.Application.Contracts.Services.Authentication;

public record AccessToken(string Token, DateTime ExpiresAt);
