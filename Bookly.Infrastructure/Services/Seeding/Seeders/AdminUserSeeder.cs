using Bookly.Domain.Constants;
using Bookly.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bookly.Infrastructure.Services.Seeding.Seeders;

public class AdminUserSeeder(UserManager<ApplicationUser> userManager)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        const string email = "admin@bookly.com";
        const string password = "Admin@123456";
        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = "Bookly",
                LastName = "Admin",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
        {
            var result = await userManager.AddToRoleAsync(admin, Roles.Admin);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}