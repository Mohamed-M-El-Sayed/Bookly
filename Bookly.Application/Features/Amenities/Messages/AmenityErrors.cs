using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Amenities.Messages;

public static class AmenityErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("Amenity.NotFound", "Amenity was not found.");
    public static Error NameAlreadyExists() =>
       ErrorFactory.Conflict("Amenity.NameAlreadyExists", "An amenity with the same name already exists.");
    public static Error InvalidSelection() =>
        ErrorFactory.Validation("Amenity.InvalidSelection", "One or more selected amenities are invalid or inactive.");
    public static Error InUse() =>
        ErrorFactory.Conflict("Amenity.InUse",
        "The amenity cannot be deleted because it is used by a hotel or a room type.");
}
