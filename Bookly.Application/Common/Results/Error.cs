namespace Bookly.Application.Common.Results;

public sealed record Error(string Code, string Message);
public static class Errors
{
    public static Error NotFound(string? message = null)
        => new("NotFound", message ?? "The requested resource was not found.");

    public static Error Conflict(string? message = null)
        => new("Conflict", message ?? "The request conflicts with the current state.");

    public static Error Validation(string? message = null)
        => new("Validation", message ?? "One or more validation errors occurred.");

    public static Error Unauthorized(string? message = null)
        => new("Unauthorized", message ?? "You are not authorized.");

    public static Error Forbidden(string? message = null)
        => new("Forbidden", message ?? "You do not have permission to perform this action.");

    public static Error InternalServer(string? message = null)
        => new("InternalServer", message ?? "An unexpected error occurred.");
}

