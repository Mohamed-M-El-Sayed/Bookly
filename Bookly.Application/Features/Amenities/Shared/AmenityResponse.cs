using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Amenities.Shared;

public record AmenityResponse(int Id,
    string NameEn,
    string NameAr,
    string? Icon,
    AmenityScope Scope,
    int CategoryId,
    bool IsActive);


