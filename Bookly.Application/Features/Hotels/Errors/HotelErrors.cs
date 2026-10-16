using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Hotels;

public static class HotelErrors
{
    public static Error NotFound() =>
        Errors.NotFound("Hotel.NotFound", "Hotel not found.");
}