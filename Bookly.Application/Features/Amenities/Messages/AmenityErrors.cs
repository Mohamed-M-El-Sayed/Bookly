using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Amenities.Messages;

public static class AmenityErrors
{
    public static Error NameAlreadyExists() =>
       ErrorFactory.Conflict("Amenity.NameAlreadyExists", "An amenity with the same name already exists.");
}
