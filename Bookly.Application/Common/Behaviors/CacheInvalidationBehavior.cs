using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

public class CacheInvalidationBehavior<TRequest, TResponse>(ICacheInvalidationService cacheInvalidationService)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is not ICacheInvalidationRequest invalidationRequest)
            return response;

        if (response is Result result && result.IsFailure)
            return response;

        if (invalidationRequest.Tags.Count == 0)
            return response;

        await cacheInvalidationService.EvictByTagsAsync(
            invalidationRequest.Tags,
            cancellationToken);

        return response;
    }
}