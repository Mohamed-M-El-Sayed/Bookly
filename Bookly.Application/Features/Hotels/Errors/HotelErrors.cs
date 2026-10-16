using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Hotels.Errors;

public static class HotelErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("Hotel.NotFound", "Hotel not found.");
}