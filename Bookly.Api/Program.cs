using Bookly.Api.Middlewares;
using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Application.DependancyInjection;
using Bookly.Infrastructure.DependancyInjection;
using Bookly.Infrastructure.Services.Authentication;
using Bookly.Infrastructure.Services.BackgroundJobs.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Bookly.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicaiton(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);
            builder.Services.AddTransient<ErrorHandlingMiddleware>();

            builder.Services.AddControllers().AddJsonOptions(options =>
                   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            builder.Services.AddEndpointsApiExplorer();

            // Configure JWT Authentication.
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtOptions = builder.Configuration
                        .GetSection(JwtOptions.SectionName)
                        .Get<JwtOptions>()!;

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                jwtOptions.Key))
                    };
                });

            // Configure Swagger and JWT authorization.
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(
                    AppContext.BaseDirectory,
                    xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT access token."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            builder.Services.AddLocalization();

            string[] supportedCultures = ["en", "ar"];

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var backgroundJobService =
                    scope.ServiceProvider.GetRequiredService<IBackgroundJobService>();

                backgroundJobService.AddOrUpdate<AvailabilityInitializationJob>(
                    "initialize-room-availability",
                    job => job.ExecuteAsync(CancellationToken.None),
                    Cron.Daily());
            }

            // Localization: first in the pipeline, so everything after it sees the request's culture
            app.UseRequestLocalization(options =>
            {
                options.SetDefaultCulture("en")
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);

                options.ApplyCurrentCultureToResponseHeaders = true;   // adds a Content-Language header
            });

            app.UseHangfireDashboard();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
