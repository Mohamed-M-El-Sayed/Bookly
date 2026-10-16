using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Auth.Messages;

public static class AuthErrors
{
    public static Error Unauthorized() =>
        ErrorFactory.Unauthorized("Auth.Unauthorized", "You are not authorized to perform this action.");
    public static Error InvalidCredentials() =>
        ErrorFactory.Unauthorized("Auth.InvalidCredentials", "Email or password is incorrect.");

    public static Error EmailNotConfirmed() =>
        ErrorFactory.Unauthorized("Auth.EmailNotConfirmed", "Please confirm your email before logging in.");

    public static Error InvalidToken() =>
        ErrorFactory.Unauthorized("Auth.InvalidToken", "Invalid or expired token.");

    public static Error InvalidConfirmationLink() =>
        ErrorFactory.Validation("Auth.InvalidConfirmationLink", "Invalid or expired confirmation link.");

    public static Error InvalidResetLink() =>
        ErrorFactory.Validation("Auth.InvalidResetLink", "Invalid or expired reset link.");

    public static Error PasswordReused() =>
        ErrorFactory.Validation("Auth.PasswordReused", "You cannot reuse your previous passwords.");

    public static Error RegistrationFailed() =>
        ErrorFactory.Internal("Auth.RegistrationFailed", "Registration could not be completed.");

    public static Error LoginFailed() =>
        ErrorFactory.Internal("Auth.LoginFailed", "Failed to update user with refresh token.");
    public static Error PasswordCompromised() =>
        ErrorFactory.Validation("Auth.PasswordCompromised", "The password you have chosen is compromised. Please choose a different password.");
}