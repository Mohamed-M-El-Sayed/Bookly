namespace Bookly.Application.Common.Results;

public sealed record MessageResponse(string Key, string DefaultMessage, object[]? Args = null);
