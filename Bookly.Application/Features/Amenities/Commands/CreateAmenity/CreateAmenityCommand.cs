using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using Bookly.Domain.Enums;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.CreateAmenity;

public record CreateAmenityCommand(
    string NameEn,
    string NameAr,
    int AmenityCategoryId,
    string? Icon,
    AmenityScope Scope
) : IRequest<Result<CreateAmenityResponse>>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelAmenities, CacheTags.RoomTypeAmenities];
}