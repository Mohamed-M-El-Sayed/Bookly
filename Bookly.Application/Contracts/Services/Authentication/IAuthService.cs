using Bookly.Application.Common.Results;
using Bookly.Application.Features.Auth.Shared;

namespace Bookly.Application.Contracts.Services.Authentication;

public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<Result> RegisterAsync(string email, string password, string firstName, string lastName,
   CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default);
    Task<Result> ForgetPasswordAsync(string email, CancellationToken cancellationToken = default);
    Task<Result> ResetPasswordAsync(string userId, string token, string newPassword, CancellationToken cancellationToken = default);
    Task<Result> ResendConfrimationEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default);
}
