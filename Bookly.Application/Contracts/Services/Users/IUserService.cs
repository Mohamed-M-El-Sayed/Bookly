using Bookly.Application.Common.Results;

namespace Bookly.Application.Contracts.Services.Users;

public interface IUserService
{
    Task<Result> AssignRoleAsync(string userId, string role, CancellationToken cancellationToken = default);
    Task<Result> UnAssignRoleAsync(string userId, string role, CancellationToken cancellationToken = default);
}
