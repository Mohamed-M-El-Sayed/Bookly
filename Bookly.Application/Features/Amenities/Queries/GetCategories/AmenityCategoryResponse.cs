namespace Bookly.Application.Features.Amenities.Queries.GetCategories;

public record AmenityCategoryResponse(int Id, string NameEn, string NameAr, int SortOrder, bool IsActive);

