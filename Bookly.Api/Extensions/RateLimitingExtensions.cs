using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Bookly.Api.Extensions;


public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Global limit: per authenticated user, otherwise per IP.
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
            {
                var key = ctx.User.Identity?.IsAuthenticated == true
                    ? ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? ctx.Connection.RemoteIpAddress?.ToString()
                        ?? "unknown"
                    : ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetSlidingWindowLimiter(
                    key,
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });

            // Authentication endpoints: per IP.
            options.AddPolicy("auth", ctx =>
                RateLimitPartition.GetFixedWindowLimiter(
                    ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));

            // Booking creation: per user.
            options.AddPolicy("booking", ctx =>
            {
                var key = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? ctx.Connection.RemoteIpAddress?.ToString()
                    ?? "unknown";

                return RateLimitPartition.GetTokenBucketLimiter(
                    key,
                    _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 10,
                        TokensPerPeriod = 5,
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(
                    MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString();
                }

                await context.HttpContext.Response.WriteAsJsonAsync(
                    new { error = "Too many requests. Try again later." },
                    cancellationToken);
            };
        });

        return services;
    }
}
