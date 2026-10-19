using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.AmenityCategories.Messages;

public static class AmenityCategoryErrors
{
    public static Error CategoryNotFound() =>
        ErrorFactory.NotFound("AmenityCategory.NotFound", "The amenity category was not found.");
    public static Error CategoryNameAlreadyExists() =>
        ErrorFactory.Conflict("AmenityCategory.NameAlreadyExists",
            "An amenity category with the same name already exists.");
    public static Error CategoryHasAmenities() =>
        ErrorFactory.Conflict("AmenityCategory.HasAmenities",
            "The amenity category cannot be deleted because it has associated amenities.");
    public static Error CategoryInUse() =>
        ErrorFactory.Conflict("AmenityCategory.CategoryInUse",
        "Cannot deactivate a category that has active amenities.");
}