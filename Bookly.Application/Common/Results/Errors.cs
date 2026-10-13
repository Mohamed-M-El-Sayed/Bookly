namespace Bookly.Application.Common.Results;

public static class Errors
{
    public static Error NotFound(string code, string message, params object[] args)
        => new(code, message, ErrorType.NotFound, args);

    public static Error Conflict(string code, string message, params object[] args)
        => new(code, message, ErrorType.Conflict, args);

    public static Error Validation(string code, string message, params object[] args)
        => new(code, message, ErrorType.Validation, args);

    public static Error Unauthorized(string code, string message, params object[] args)
        => new(code, message, ErrorType.Unauthorized, args);

    public static Error Forbidden(string code, string message, params object[] args)
        => new(code, message, ErrorType.Forbidden, args);

    public static Error Internal(string code, string message, params object[] args)
        => new(code, message, ErrorType.Internal, args);

    // Several validation failures carried in one error
    public static Error ValidationFailed(IEnumerable<Error> details)
        => new("Validation.Failed", "One or more validation errors occurred.",
               ErrorType.Validation, null, details.ToList());
}
