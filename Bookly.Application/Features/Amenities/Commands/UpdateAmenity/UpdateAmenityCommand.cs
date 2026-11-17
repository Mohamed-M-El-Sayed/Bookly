using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using Bookly.Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Amenities.Commands.UpdateAmenity;

public class UpdateAmenityCommand : IRequest<Result>, ICacheInvalidationRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string NameEn { get; set; } = default!;
    public string NameAr { get; set; } = default!;
    public int AmenityCategoryId { get; set; } = default!;
    public string? Icon { get; set; }
    public AmenityScope Scope { get; set; }

    public IReadOnlyCollection<string> Tags => [CacheTags.HotelAmenities];
}
