namespace Bookly.Application.Contracts.Services.Caching;

public interface ICacheInvalidationRequest
{
    IReadOnlyList<string> Tags { get; set; }
}
