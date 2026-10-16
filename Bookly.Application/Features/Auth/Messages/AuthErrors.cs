using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Auth.Messages;

public static class AuthErrors
{
    public static Error InvalidCredentials() =>
        Errors.Unauthorized("Auth.InvalidCredentials", "Email or password is incorrect.");

    public static Error EmailNotConfirmed() =>
        Errors.Unauthorized("Auth.EmailNotConfirmed", "Please confirm your email before logging in.");

    public static Error InvalidToken() =>
        Errors.Unauthorized("Auth.InvalidToken", "Invalid or expired token.");

    public static Error InvalidConfirmationLink() =>
        Errors.Validation("Auth.InvalidConfirmationLink", "Invalid or expired confirmation link.");

    public static Error InvalidResetLink() =>
        Errors.Validation("Auth.InvalidResetLink", "Invalid or expired reset link.");

    public static Error PasswordReused() =>
        Errors.Validation("Auth.PasswordReused", "You cannot reuse your previous passwords.");

    public static Error RegistrationFailed() =>
        Errors.Internal("Auth.RegistrationFailed", "Registration could not be completed.");

    public static Error LoginFailed() =>
        Errors.Internal("Auth.LoginFailed", "Failed to update user with refresh token.");
    public static Error PasswordCompromised() =>
        Errors.Validation("Auth.PasswordCompromised", "The password you have chosen is compromised. Please choose a different password.");
}