using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Bookly.Infrastructure.Services.Authentication;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions jwtOptions;
    private readonly UserManager<ApplicationUser> _userManager;
    public TokenService(IOptions<JwtOptions> options , UserManager<ApplicationUser> userManager)
    {
        jwtOptions = options.Value;
        _userManager = userManager;
    }
    public async Task<AccessToken> GenerateAccessToken(ApplicationUser user)
    {
        var cliams = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier ,user.Id.ToString()) , 
            new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()) , 
            new(JwtRegisteredClaimNames.Email , user.Email!) , 
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            cliams.Add(new Claim(ClaimTypes.Role, role));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenExpirationMinutes);
        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: cliams,
            expires: expiresAt,
            signingCredentials: signingCredentials
        );
        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, // We want to get claims from expired token
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validated);

            // Reject tokens signed with a different algorithm
            if (validated is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }
    public RefreshTokenn GenerateRefreshToken()
    {
        return new RefreshTokenn(
            Guid.NewGuid().ToString(),
            DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpirationDays)); 
    }
}
