using Bookly.Api.Resources;
using Bookly.Api.Responses;
using Bookly.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bookly.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToResponse<T>(
        this Result<T> result,
        int statusCode = StatusCodes.Status200OK,
        IStringLocalizer<SharedResource>? localizer = null)
    {
        if (result.IsFailure)
        {
            var errorStatus = GetStatusCode(result.Error!);

            return ToObjectResult(errorStatus,
                new ApiResponse<T>(errorStatus, false, default, ToApiErrors(result.Error!, localizer)));
        }

        // Handlers return a message key; the text is translated here
        if (result.Value is MessageResponse message)
        {
            var text = Localize(message.Key, message.DefaultMessage, message.Args, localizer);

            return ToObjectResult(statusCode,
                new ApiResponse<MessageDto>(statusCode, true, new MessageDto(text), null));
        }

        return ToObjectResult(statusCode,
            new ApiResponse<T>(statusCode, true, result.Value, null));
    }

    public static IActionResult ToResponse(
        this Result result,
        int statusCode = StatusCodes.Status204NoContent,
        IStringLocalizer<SharedResource>? localizer = null)
    {
        if (result.IsFailure)
        {
            var errorStatus = GetStatusCode(result.Error!);

            return ToObjectResult(errorStatus,
                new ApiResponse(errorStatus, false, ToApiErrors(result.Error!, localizer)));
        }

        // A 204 response must not have a body
        if (statusCode == StatusCodes.Status204NoContent)
            return new NoContentResult();

        return ToObjectResult(statusCode, new ApiResponse(statusCode, true, null));
    }

    private static ObjectResult ToObjectResult(int statusCode, object body)
        => new(body) { StatusCode = statusCode };

    private static int GetStatusCode(Error error) => error.Type switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.Internal => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    // A validation error carries one entry per failed rule in Details
    private static ApiError[] ToApiErrors(Error error, IStringLocalizer<SharedResource>? localizer)
    {
        IReadOnlyList<Error> errors = error.Details is { Count: > 0 } ? error.Details : [error];

        return errors
            .Select(e => new ApiError(e.Code, Localize(e.Code, e.Message, e.Args, localizer)))
            .ToArray();
    }

    // One place for translating both errors and messages
    private static string Localize(
        string key,
        string defaultMessage,
        object[]? args,
        IStringLocalizer<SharedResource>? localizer)
    {
        var hasArgs = args is { Length: > 0 };

        // The English fallback also gets its placeholders filled in
        var fallback = hasArgs ? string.Format(defaultMessage, args!) : defaultMessage;

        if (localizer is null)
            return fallback;

        var entry = hasArgs ? localizer[key, args!] : localizer[key];

        return entry.ResourceNotFound ? fallback : entry.Value;
    }
}