using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Contracts.Services.Email;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Auth.Shared;
using Bookly.Domain.Constants;
using Bookly.Domain.Entities.Identity;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Bookly.Infrastructure.Services.Authentication;

public class AuthService(
    ITokenService tokenService,
    UserManager<ApplicationUser> userManager,
    AppDbContext appDbContext,
    IConfiguration configuration,
    IEmailService emailService,
    PasswordHistoryService historyService) : IAuthService
{
    public async Task<Result<AuthResponse>> LoginAsync(
        string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        // Check the password first, so an unconfirmed account isn't revealed to someone without it
        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return Result<AuthResponse>.Failure(AuthErrors.InvalidCredentials());

        if (!user.EmailConfirmed)
            return Result<AuthResponse>.Failure(AuthErrors.EmailNotConfirmed());

        var accessToken = await tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var roles = await userManager.GetRolesAsync(user);

        user.AddRefreshToken(refreshToken.Token, refreshToken.ExpiresAt);

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return Result<AuthResponse>.Failure(AuthErrors.LoginFailed());

        return Result<AuthResponse>.Success(new AuthResponse(
            new UserResponse(user.Id, user.Email!, roles),
            accessToken.Token,
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
        {
            await HandleExistingUserAsync(existingUser, cancellationToken);
            return Result.Success(); // same outcome as a new registration
        }

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
            // A parallel request registered the same email a moment ago
            if (result.Errors.Any(e => e.Code is "DuplicateEmail" or "DuplicateUserName"))
                return Result.Success();

            return Result.Failure(
                ErrorFactory.Validation("code", string.Join(", ", result.Errors.Select(e => e.Description))));
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Customer);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Result.Failure(AuthErrors.RegistrationFailed());
        }

        await SendConfirmationEmailAsync(user, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(
        string accessToken, string refreshToken, CancellationToken cancellationToken = default)
    {
        var claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (claimsPrincipal is null)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());

        var userIdClaim =
            claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ??
            claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());

        var user = await appDbContext.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());

        var stored = user.FindRefreshToken(refreshToken);
        if (stored is null)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());

        // A revoked token is being reused: possible theft, end all sessions
        if (stored.IsRevoked)
        {
            user.RevokeAllRefreshTokens();
            await appDbContext.SaveChangesAsync(cancellationToken);
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());
        }

        if (stored.IsExpired)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());

        if (!user.EmailConfirmed || await userManager.IsLockedOutAsync(user))
            return Result<AuthResponse>.Failure(AuthErrors.InvalidToken());

        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newAccessToken = await tokenService.GenerateAccessToken(user);

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

    public async Task<Result> ConfirmEmailAsync(
        string userId, string token, CancellationToken cancellationToken = default)
    {
        // Unknown user and bad token give the same error, so user ids can't be probed
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(AuthErrors.InvalidConfirmationLink());

        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Failure(AuthErrors.InvalidConfirmationLink());
        }

        var result = await userManager.ConfirmEmailAsync(user, decodedToken);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(AuthErrors.InvalidConfirmationLink());
    }

    public async Task<Result> ForgetPasswordAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !user.EmailConfirmed)
            return Result.Success(); // same result, to prevent email enumeration

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var resetLink = $"{configuration["AppSettings:BaseUrl"]}" +
            $"api/auth/reset-password?userId={user.Id}&token={encodedToken}";

        var body = $@"
        <h2>Reset your password</h2>
        <p>Hi {WebUtility.HtmlEncode(user.FirstName)},</p>
        <p>We received a request to reset your password. Click
        <a href='{resetLink}'>here</a> to choose a new one.</p>
        <p>If you didn't request this, you can ignore this email.</p>";

        await emailService.SendEmailAsync(user.Email!, "Reset your password", body, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(
        string userId, string token, string newPassword,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(userId, out var id))
            return Result.Failure(AuthErrors.InvalidResetLink());

        var user = await appDbContext.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
            return Result.Failure(AuthErrors.InvalidResetLink());

        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Failure(AuthErrors.InvalidResetLink());
        }

        // Verify the token BEFORE the reuse check, without consuming it.
        // Otherwise anyone who knows a user id could learn which passwords that user has used.
        var isTokenValid = await userManager.VerifyUserTokenAsync(
            user,
            userManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<ApplicationUser>.ResetPasswordTokenPurpose,
            decodedToken);

        if (!isTokenValid)
            return Result.Failure(AuthErrors.InvalidResetLink());

        var isPasswordReused = await historyService
            .IsPasswordReusedAsync(user, newPassword, cancellationToken);

        if (isPasswordReused)
            return Result.Failure(AuthErrors.PasswordReused());

        var oldPasswordHash = user.PasswordHash;

        var result = await userManager.ResetPasswordAsync(user, decodedToken, newPassword);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
                return Result.Failure(AuthErrors.InvalidResetLink());

            return Result.Failure(
                ErrorFactory.Validation("code", string.Join(", ", result.Errors.Select(e => e.Description))));
        }

        if (!string.IsNullOrEmpty(oldPasswordHash))
            await historyService.AddPasswordToHistoryAsync(user, oldPasswordHash, cancellationToken);

        user.RevokeAllRefreshTokens();
        await appDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ResendConfrimationEmailAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        // Unknown or already confirmed: do nothing and answer the same way
        if (user is null || user.EmailConfirmed)
            return Result.Success();

        await SendConfirmationEmailAsync(user, cancellationToken);
        return Result.Success();
    }

    private async Task HandleExistingUserAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        if (!user.EmailConfirmed)
        {
            await SendConfirmationEmailAsync(user, cancellationToken);
            return;
        }

        var body = $@"
        <p>Hi {WebUtility.HtmlEncode(user.FirstName)},</p>
        <p>Someone tried to register with this email, but an account already exists.
        You can sign in or reset your password.</p>
        <p>If this wasn't you, you can ignore this message.</p>";

        await emailService.SendEmailAsync(
            user.Email!, "You already have an account", body, cancellationToken);
    }

    private async Task SendConfirmationEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var confirmationLink =
            $"{configuration["AppSettings:BaseUrl"]}api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

        var body = $@"
        <h2>Welcome {WebUtility.HtmlEncode(user.FirstName)}</h2>
        <p>Thank you for registering.</p>
        <p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";

        await emailService.SendEmailAsync(user.Email!, "Confirm your email", body, cancellationToken);
    }
}