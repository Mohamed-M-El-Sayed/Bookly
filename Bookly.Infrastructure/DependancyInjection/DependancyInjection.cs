using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Authentication;
using Bookly.Application.Contracts.Services.Availability;
using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Application.Contracts.Services.Email;
using Bookly.Application.Contracts.Services.Files;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Contracts.Services.Security;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Domain.Entities.Identity;
using Bookly.Infrastructure.Persistence.Context;
using Bookly.Infrastructure.Persistence.UnitOfWork;
using Bookly.Infrastructure.Services.Authentication;
using Bookly.Infrastructure.Services.Availability;
using Bookly.Infrastructure.Services.BackgroundJobs;
using Bookly.Infrastructure.Services.BackgroundJobs.Jobs;
using Bookly.Infrastructure.Services.Email;
using Bookly.Infrastructure.Services.Files;
using Bookly.Infrastructure.Services.Hotels;
using Bookly.Infrastructure.Services.Security;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookly.Infrastructure.DependancyInjection
{
    public static class DependancyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string wwwRootPath)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfireServer();
            services.AddDataProtection();
            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileService>(_ => new FileService(wwwRootPath));
            services.AddScoped<PasswordHistoryService>();
            services.AddHttpClient<ICompromisedPasswordService, CompromisedPasswordService>(
                client =>
                {
                    client.BaseAddress = new Uri("https://api.pwnedpasswords.com/");
                });
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IHotelAccessService, HotelAccessService>();
            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
            services.AddScoped<AvailabilityInitializationJob>();
            services.AddScoped<IAvailabilityInitializer, AvailabilityInitializer>();
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        }
    }
}
