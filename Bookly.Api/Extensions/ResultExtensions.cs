using Bookly.Api.Responses;
using Bookly.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToResponse<T>(this Result<T> result, int statusCode = StatusCodes.Status200OK)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(
                new ApiResponse<T>(
                    statusCode,
                    true,
                    result.Value,
                    null))
            {
                StatusCode = statusCode
            };
        }

        var errorStatusCode = GetStatusCode(result.Error!);

        return new ObjectResult(
            new ApiResponse<T>(
                errorStatusCode,
                false,
                default,
                new[]
                {
                    new ApiError(
                        result.Error!.Code,
                        result.Error.Message)
                }))
        {
            StatusCode = errorStatusCode
        };
    }

    public static IActionResult ToResponse(this Result result, int statusCode = StatusCodes.Status204NoContent)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(
                new ApiResponse(
                    statusCode,
                    true,
                    null))
            {
                StatusCode = statusCode
            };
        }

        var errorStatusCode = GetStatusCode(result.Error!);

        return new ObjectResult(
            new ApiResponse(
                errorStatusCode,
                false,
                new[]
                {
                    new ApiError(
                        result.Error!.Code,
                        result.Error.Message)
                }))
        {
            StatusCode = errorStatusCode
        };
    }

    private static int GetStatusCode(Error error)
    {
        return error.Code switch
        {
            "NotFound" =>
                StatusCodes.Status404NotFound,

            "Conflict" =>
                StatusCodes.Status409Conflict,

            "Validation" =>
                StatusCodes.Status400BadRequest,

            "Unauthorized" =>
                StatusCodes.Status401Unauthorized,

            "Forbidden" =>
                StatusCodes.Status403Forbidden,

            "InternalServer" =>
                StatusCodes.Status500InternalServerError,

            _ =>
                StatusCodes.Status400BadRequest
        };
    }
}