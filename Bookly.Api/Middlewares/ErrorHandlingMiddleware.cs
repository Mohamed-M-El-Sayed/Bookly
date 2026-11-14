using Bookly.Api.Responses;
using Bookly.Domain.Exceptions;
using FluentValidation;
using System.Net;

namespace Bookly.Api.Middlewares;

public class ErrorHandlingMiddleware(
    ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Exception: {Message}",
                ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception ex)
    {
        context.Response.ContentType = "application/json";

        var response = ex switch
        {
            // 400 - FluentValidation
            ValidationException validationEx =>
                new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    false,
                    validationEx.Errors.Select(e =>
                        new ApiError(
                            e.PropertyName,
                            e.ErrorMessage))),

            //// 409 - Domain business rule
            DomainException domainEx =>
                 new ApiResponse(
                     (int)HttpStatusCode.Conflict,
                     false,
                     new[]
                     {
                            new ApiError(
                                "DomainRule",
                                domainEx.Message)
                     }),

            // 500 - Unexpected exception
            _ =>
                new ApiResponse(
                    (int)HttpStatusCode.InternalServerError,
                    false,
                    new[]
                    {
                            new ApiError(
                                "InternalServerError",
                                "An unexpected error occurred.")
                    })
        };

        context.Response.StatusCode = response.StatusCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}