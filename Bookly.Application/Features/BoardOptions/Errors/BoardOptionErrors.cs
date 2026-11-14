using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.BoardOptions.Errors;

public static class BoardOptionErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("BoardOption.NotFound", "Board option was not found.");
}
