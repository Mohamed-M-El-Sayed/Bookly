namespace Bookly.Application.Contracts.Services.Caching;

public interface ICacheInvalidationService
{
    Task EvictByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default);
}
