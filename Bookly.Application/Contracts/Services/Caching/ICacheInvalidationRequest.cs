namespace Bookly.Application.Contracts.Services.Caching;

public interface ICacheInvalidationRequest
{
    IReadOnlyCollection<string> Tags { get; set; }
}
