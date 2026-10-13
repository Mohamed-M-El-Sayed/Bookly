using Bookly.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace Bookly.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        // Requests that don't return a Result keep the old behavior
        if (!typeof(Result).IsAssignableFrom(typeof(TResponse)))
            throw new ValidationException(failures);

        var error = Errors.ValidationFailed(
            failures.Select(f => Errors.Validation(f.ErrorCode, f.ErrorMessage)));

        return CreateFailure(error);
    }

    private static TResponse CreateFailure(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
            return (TResponse)(object)Result.Failure(error);

        // TResponse is Result<T>: call Result<T>.Failure(error)
        var valueType = typeof(TResponse).GetGenericArguments()[0];

        var failure = typeof(Result<>)
            .MakeGenericType(valueType)
            .GetMethod("Failure", [typeof(Error)])!;

        return (TResponse)failure.Invoke(null, [error])!;
    }
}