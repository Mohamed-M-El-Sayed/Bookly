using Bookly.Application.Contracts.Services.Caching;
using Microsoft.AspNetCore.OutputCaching;

namespace Bookly.Infrastructure.Services.Caching;

public class CacheInvalidationService(IOutputCacheStore outputCacheStore) : ICacheInvalidationService
{

    public async Task EvictByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        foreach (var tag in tags.Distinct())
        {
            await outputCacheStore.EvictByTagAsync(tag, cancellationToken);
        }
    }
}
