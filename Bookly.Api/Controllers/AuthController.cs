using Bookly.Application.Features.Auth.Commands.ConfirmEmail;
using Bookly.Application.Features.Auth.Commands.ForgetPassword;
using Bookly.Application.Features.Auth.Commands.Login;
using Bookly.Application.Features.Auth.Commands.RefreshToken;
using Bookly.Application.Features.Auth.Commands.Register;
using Bookly.Application.Features.Auth.Commands.ResendConfirmation;
using Bookly.Application.Features.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Bookly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("auth")]
public class AuthController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Authenticates a user and returns authentication tokens.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Confirms a user's email using the provided userId and token.
    /// </summary>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Gets a new access token and refresh token using the current pair.
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Sends a password reset link to the given email.
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        ForgetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Sets a new password using the token from the reset email.
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromQuery] string userId,
        [FromQuery] string token,
        [FromBody] ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        command.UserId = userId;
        command.Token = token;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Sends a new confirmation email to an unconfirmed account.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation(
        ResendConfirmationCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}