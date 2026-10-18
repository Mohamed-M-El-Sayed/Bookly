namespace Bookly.Application.Features.AmenityCategories.Queries.GetCategories;

public record AmenityCategoryResponse(int Id, string NameEn, string NameAr, int SortOrder, bool IsActive);

