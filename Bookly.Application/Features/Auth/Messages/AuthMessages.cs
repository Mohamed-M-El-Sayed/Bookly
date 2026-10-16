using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Auth.Messages;

public static class AuthMessages
{
    public static MessageResponse RegisterResponse()
        => new("Auth.Register.Success", "Registration successful. Please check your email to verify your account.");

    public static MessageResponse ForgetPasswordResponse()
        => new("Auth.ForgetPassword.Sent", "If an account exists for that email, a reset link has been sent.");
}
