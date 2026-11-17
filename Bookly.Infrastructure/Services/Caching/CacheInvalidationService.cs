using Bookly.Application.Contracts.Services.Caching;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;

namespace Bookly.Infrastructure.Services.Caching;

public class CacheInvalidationService(IOutputCacheStore outputCacheStore,
    ILogger<CacheInvalidationService> logger) : ICacheInvalidationService
{

    public async Task EvictByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        var cacheTags = tags.Distinct().ToList();
        logger.LogInformation("Starting cache invalidation for tags: {CacheTags}",
            string.Join(", ", cacheTags));

        foreach (var tag in cacheTags)
        {
            await outputCacheStore.EvictByTagAsync(tag, cancellationToken);
            logger.LogInformation("Cache invalidated successfully for tag {CacheTag}.", tag);
        }
        logger.LogInformation("Cache invalidation completed successfully for {TagCount} tags.", cacheTags.Count);

    }
}
