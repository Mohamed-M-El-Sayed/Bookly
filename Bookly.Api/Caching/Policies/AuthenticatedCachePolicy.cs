using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Net.Http.Headers;

namespace Bookly.Api.Caching.Policies;

public sealed class AuthenticatedCachePolicy(ILogger<AuthenticatedCachePolicy> logger) : IOutputCachePolicy
{
    public ValueTask CacheRequestAsync(
        OutputCacheContext ctx,
        CancellationToken ct)
    {
        var isSafe =
            HttpMethods.IsGet(ctx.HttpContext.Request.Method) ||
            HttpMethods.IsHead(ctx.HttpContext.Request.Method);

        ctx.EnableOutputCaching = true;
        ctx.AllowCacheLookup = isSafe;
        ctx.AllowCacheStorage = isSafe;
        ctx.AllowLocking = true;
        ctx.CacheVaryByRules.QueryKeys = "*";

        logger.LogInformation(
            "Output Cache Request: Authenticated={IsAuthenticated}, User={UserId}, Lookup={Lookup}, Storage={Storage}",
            ctx.HttpContext.User.Identity?.IsAuthenticated,
            ctx.HttpContext.User.Identity?.Name,
            ctx.AllowCacheLookup,
            ctx.AllowCacheStorage);

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(
        OutputCacheContext ctx,
        CancellationToken ct)
        => ValueTask.CompletedTask;

    public ValueTask ServeResponseAsync(
        OutputCacheContext ctx,
        CancellationToken ct)
    {
        var response = ctx.HttpContext.Response;
        var hasSetCookie = response.Headers.ContainsKey(HeaderNames.SetCookie);

        if (response.StatusCode != StatusCodes.Status200OK || hasSetCookie)
        {
            ctx.AllowCacheStorage = false;
        }

        logger.LogInformation(
            "Output Cache Response: StatusCode={StatusCode}, HasSetCookie={HasSetCookie}, StorageAllowed={StorageAllowed}",
            response.StatusCode,
            hasSetCookie,
            ctx.AllowCacheStorage);

        return ValueTask.CompletedTask;
    }
}
