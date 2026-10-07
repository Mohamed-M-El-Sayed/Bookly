using Microsoft.AspNetCore.Identity;

namespace Bookly.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    private readonly List<PasswordHistory> _passwordHistories = [];
    public IReadOnlyCollection<PasswordHistory> PasswordHistories => _passwordHistories.AsReadOnly();
    public void AddRefreshToken(string token , DateTime expiresAt)
    {
        _refreshTokens.Add(new RefreshToken(token , expiresAt));
    }
    public void RevokeAllRefreshTokens()
    {
        foreach (var refreshToken in _refreshTokens.Where(t => !t.IsRevoked))
        {
            refreshToken.Revoke();
        }
    }
    public RefreshToken? FindRefreshToken(string token)
       => _refreshTokens.FirstOrDefault(t => t.Token == token);
    public bool RotateRefreshToken(string oldToken, string newToken, DateTime newExpiresAt)
    {
        var current = FindRefreshToken(oldToken);
        if (current is null || !current.IsActive)
            return false;

        current.Revoke();
        AddRefreshToken(newToken, newExpiresAt);
        return true;
    }
    public void AddPasswordHistory(string passwordHash)
    {
        _passwordHistories.Add(
            new PasswordHistory(Id, passwordHash));
    }
    public void RemovePasswordHistory(PasswordHistory passwordHistory)
    {
        _passwordHistories.Remove(passwordHistory);
    }
}
