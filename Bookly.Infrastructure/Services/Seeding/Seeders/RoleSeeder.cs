using Bookly.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Bookly.Infrastructure.Services.Seeding.Seeders;

public class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var roles = new[]
        {
            Roles.Admin ,
            Roles.Manager ,
            Roles.Customer
        };
        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;
            var result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role '{role}': " +
                   string.Join(", ", result.Errors.Select(e => e.Description)));
            }

        }
    }
}
