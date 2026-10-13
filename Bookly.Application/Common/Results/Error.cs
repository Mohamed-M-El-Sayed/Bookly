namespace Bookly.Application.Common.Results;

public sealed record Error(
    string Code,                           // stable key, e.g. "Auth.InvalidCredentials"
    string Message,                        // English default: used for logs and as a fallback
    ErrorType Type,
    object[]? Args = null,                 // values for placeholders in a translated message
    IReadOnlyList<Error>? Details = null); // individual failures, used by validation
