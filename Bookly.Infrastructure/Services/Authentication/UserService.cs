using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Users.Errors;
using Bookly.Domain.Constants;
using Bookly.Domain.Entities.Identity;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Authentication;

public class UserService(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    AppDbContext dbContext) : IUserService
{
    public async Task<Result> AssignRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound());
        if (!await roleManager.RoleExistsAsync(role))
            return Result.Failure(UserErrors.RoleNotFound());
        // Check if the user is already in the role
        if (await userManager.IsInRoleAsync(user, role))
            return Result.Success();
        var result = await userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            return Result.Failure(ErrorFactory.Validation(
                "code", string.Join(", ", result.Errors.Select(e => e.Description))));

        await ForceReauthenticationAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);


        return Result.Success();

    }
    public async Task<Result> UnAssignRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound());
        if (!await roleManager.RoleExistsAsync(role))
            return Result.Failure(UserErrors.RoleNotFound());
        if (role == Roles.Customer || !await userManager.IsInRoleAsync(user, role))
            return Result.Success();

        var result = await userManager.RemoveFromRoleAsync(user, role);
        if (!result.Succeeded)
            return Result.Failure(ErrorFactory.Validation(
                "code", string.Join(", ", result.Errors.Select(e => e.Description))));

        if (!await userManager.IsInRoleAsync(user, Roles.Customer))
            await userManager.AddToRoleAsync(user, Roles.Customer);

        await ForceReauthenticationAsync(user, cancellationToken);
        return Result.Success();
    }
    private async Task ForceReauthenticationAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var withTokens = await dbContext.Users
            .Include(u => u.RefreshTokens)
            .FirstAsync(u => u.Id == user.Id, cancellationToken);

        withTokens.RevokeAllRefreshTokens();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
