using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Hotels.Errors;

public static class HotelErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("Hotel.NotFound", "Hotel not found.");
    public static Error NotManager() =>
           ErrorFactory.Forbidden("Hotel.NotManager", "You are not authorized to manage this hotel.");

}