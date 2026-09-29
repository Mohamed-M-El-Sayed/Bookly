namespace Bookly.Api.Responses;

public record ApiResponse(int StatusCode, bool IsSuccess, IEnumerable<ApiError>? Errors);
public record ApiResponse<T>(int StatusCode, bool IsSuccess, T? Data, IEnumerable<ApiError>? Errors);
public record ApiError(string Code, string Message);