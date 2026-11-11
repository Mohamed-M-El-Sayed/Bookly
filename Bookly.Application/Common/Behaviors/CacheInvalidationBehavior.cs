using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Common.Behaviors;

public class CacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICacheInvalidationService _cacheInvalidationService;
    public CacheInvalidationBehavior(ICacheInvalidationService cacheInvalidationService)
    {
        _cacheInvalidationService = cacheInvalidationService;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is ICacheInvalidationRequest invalidationRequest)
        {
            if (response is Result result && result.IsFailure)
                return response;
            await _cacheInvalidationService.EvictByTagsAsync(invalidationRequest.Tags, cancellationToken);
        }
        return response;
    }
}
