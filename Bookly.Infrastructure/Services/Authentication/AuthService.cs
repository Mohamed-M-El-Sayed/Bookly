using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Contracts.Services.Email;
using Bookly.Application.Features.Auth.Shared;
using Bookly.Domain.Constants;
using Bookly.Domain.Entities.Identity;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookly.Infrastructure.Services.Authentication;

public class AuthService(ITokenService tokenService, 
    UserManager<ApplicationUser> userManager ,
    AppDbContext appDbContext ,
    IConfiguration configuration ,
    IEmailService emailService ,
    PasswordHistoryService historyService) : IAuthService
{
    public async Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user?.EmailConfirmed is false)
            return Result<AuthResponse>
                .Failure(Errors.Unauthorized("Please confirm your email before logging in."));
        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return Result<AuthResponse>
                .Failure(Errors.Unauthorized("Email or password is incorrect."));
        var accessToken = await tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var roles = await userManager.GetRolesAsync(user);
        user.AddRefreshToken(refreshToken.Token, refreshToken.ExpiresAt);
        var updateResult = await userManager.UpdateAsync(user); 
        if(!updateResult.Succeeded)
        {
            return Result<AuthResponse>
                .Failure(Errors.InternalServer("Failed to update user with refresh token."));
        }
        return Result<AuthResponse>.Success(new AuthResponse(
            new UserResponse(user.Id, user.Email!, roles)
            , accessToken.Token,
            accessToken.ExpiresAt,
            refreshToken.Token,
            refreshToken.ExpiresAt));

    }
    public async Task<Result> RegisterAsync(
        string email, string password, string firstName, string lastName,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return Result.Failure(
                Errors.Conflict("Email is already registered."));
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return Result.Failure(
                Errors.Validation(
                    string.Join(", ", result.Errors.Select(e => e.Description))));
        }
        var roleResult = await userManager.AddToRoleAsync(user, Roles.Customer);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Result.Failure(Errors.InternalServer("Registration could not be completed."));
        }
        await SendConfirmationEmailAsync(user , cancellationToken); 
        return Result.Success();

    }
    public async Task<Result<AuthResponse>> RefreshTokenAsync(
        string accessToken, string refreshToken, CancellationToken cancellationToken = default)
    {
        const string invalid = "Invalid or expired token.";

        var claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (claimsPrincipal is null)
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));

        var userIdClaim =
            claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ??
            claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));

        var user = await appDbContext.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));

        var stored = user.FindRefreshToken(refreshToken);
        if (stored is null)
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));
        // Revoke all refresh tokens if a revoked token is reused, indicating possible token theft.
        if (stored.IsRevoked)
        {
            user.RevokeAllRefreshTokens();
            await appDbContext.SaveChangesAsync(cancellationToken);
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));
        }

        if (stored.IsExpired)
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));

        if (!user.EmailConfirmed || await userManager.IsLockedOutAsync(user))
            return Result<AuthResponse>.Failure(Errors.Unauthorized(invalid));

        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newAccessToken = await tokenService.GenerateAccessToken(user);
        // Rotate the refresh token after a successful refresh to prevent reuse.
        user.RotateRefreshToken(refreshToken, newRefreshToken.Token, newRefreshToken.ExpiresAt);
        await appDbContext.SaveChangesAsync(cancellationToken);

        var roles = await userManager.GetRolesAsync(user);

        return Result<AuthResponse>.Success(new AuthResponse(
            new UserResponse(user.Id, user.Email!, roles),
            newAccessToken.Token,
            newAccessToken.ExpiresAt,
            newRefreshToken.Token,
            newRefreshToken.ExpiresAt));
    }
    public async Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(Errors.NotFound("User not found."));
        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Failure(Errors.Validation("Invalid or expired confirmation link."));
        }
        var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        return result.Succeeded
               ? Result.Success()
               : Result.Failure(Errors.Validation("Invalid or expired confirmation link."));
    }
    public async Task<Result> ForgetPasswordAsync(string email , CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if(user is null || !user.EmailConfirmed)
        {
            // Return the same result to prevent email enumeration.
            return Result.Success();
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var resetLink = $"{configuration["AppSettings:BaseUrl"]}" +
            $"api/auth/reset-password?userId={user.Id}&token={encodedToken}";
        var body = $@"
        <h2>Reset your password</h2>
        <p>Hi {user.FirstName},</p>
        <p>We received a request to reset your password. Click
        <a href='{resetLink}'>here</a> to choose a new one.</p>
        <p>If you didn't request this, you can ignore this email.</p>";
        await emailService.SendEmailAsync(user.Email!, "Reset your password", body, cancellationToken);
        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(
        string userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        const string invalidLink = "Invalid or expired reset link.";
        if (!Guid.TryParse(userId, out var id))
        {
            return Result.Failure(Errors.Validation(invalidLink));
        }
        var user = await appDbContext.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null)
        {
            return Result.Failure(Errors.Validation(invalidLink));
        }
        var isPasswordReused = await historyService
            .IsPasswordReusedAsync(user, newPassword, cancellationToken);
        if (isPasswordReused)
        {
            return Result.Failure(
                Errors.Validation("You cannot reuse your previous passwords."));
        }
        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Failure(Errors.Validation(invalidLink));
        }

        var oldPasswordHash = user.PasswordHash;

        var result = await userManager.ResetPasswordAsync(
            user,
            decodedToken,
            newPassword);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
            {
                return Result.Failure(Errors.Validation(invalidLink));
            }

            return Result.Failure(
                Errors.Validation(
                    string.Join(", ", result.Errors.Select(e => e.Description))));
        }
        if (!string.IsNullOrEmpty(oldPasswordHash))
        {
            await historyService.AddPasswordToHistoryAsync(
                user,
                oldPasswordHash,
                cancellationToken);
        }
        user.RevokeAllRefreshTokens();
        await appDbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> ResendConfrimationEmailAsync(string email , CancellationToken cancellationToken = default)
    {
       var user =  await userManager.FindByEmailAsync(email);
        // Do nothing if the email is unknown or already confirmed.
        // Return the same response to prevent email discovery.
        if (user is null || user.EmailConfirmed)
            return Result.Success();
        await SendConfirmationEmailAsync(user, cancellationToken);
        return Result.Success();
    }
    private async Task SendConfirmationEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var confirmationLink = $"{configuration["AppSettings:BaseUrl"]}api/auth/confirm-email?userId={user.Id}&token={encodedToken}";
        var body = $@"
        <h2>Welcome {user.FirstName}</h2>

        <p>Thank you for registering.</p>

        <p>
            Please confirm your email by clicking
            <a href='{confirmationLink}'>here</a>.
        </p>";
        await emailService.SendEmailAsync(
        user.Email!,
        "Confirm your email",
        body,
        cancellationToken);

    }

}

