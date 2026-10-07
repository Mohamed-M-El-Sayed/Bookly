using Bookly.Domain.Entities.Identity;
using System.Security.Claims;

namespace Bookly.Application.Contracts.Services.Authentication
{
    public interface ITokenService
    {
        Task<AccessToken> GenerateAccessToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        RefreshTokenn GenerateRefreshToken();

    }
}
