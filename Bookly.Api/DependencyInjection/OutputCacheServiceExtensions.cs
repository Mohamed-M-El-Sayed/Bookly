using Bookly.Api.Caching.Policies;
using Bookly.Application.Contracts.Services.Caching;

namespace Bookly.Api.DependencyInjection;

public static class OutputCacheServiceExtensions
{
    public static IServiceCollection AddOutputCaching(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy(
                "Reviews",
                policy => policy
                    .AddPolicy<AuthenticatedCachePolicy>()
                    .Expire(TimeSpan.FromMinutes(5))
                    .Tag(CacheTags.HotelReviews),
                excludeDefaultPolicy: true);

            options.AddPolicy(
                "HotelAmenities",
                policy => policy
                    .AddPolicy<AuthenticatedCachePolicy>()
                    .Expire(TimeSpan.FromMinutes(30))
                    .Tag(CacheTags.HotelAmenities),
                excludeDefaultPolicy: true);

            options.AddPolicy(
                "RoomTypeAmenities",
                policy => policy
                    .AddPolicy<AuthenticatedCachePolicy>()
                    .Expire(TimeSpan.FromMinutes(30))
                    .Tag(CacheTags.RoomTypeAmenities),
                excludeDefaultPolicy: true);

            options.AddPolicy(
                "HotelSearch",
                policy => policy
                    .AddPolicy<AuthenticatedCachePolicy>()
                    .Expire(TimeSpan.FromSeconds(30))
                    .SetVaryByQuery("*")
                    .Tag(CacheTags.HotelSearch),
                excludeDefaultPolicy: true);
        });

        return services;
    }

    public static IServiceCollection AddRedisOutputCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration =
                configuration.GetConnectionString("Redis");

            options.InstanceName = "Bookly:";
        });

        return services;
    }
}