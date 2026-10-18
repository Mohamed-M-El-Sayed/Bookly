using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Amenities.Messages;

public static class AmenityErrors
{
    public static Error CategoryNotFound() =>
        ErrorFactory.NotFound("AmenityCategory.NotFound", "The amenity category was not found.");
    public static Error CategoryNameAlreadyExists() =>
        ErrorFactory.Conflict("AmenityCategory.NameAlreadyExists",
            "An amenity category with the same name already exists.");
}