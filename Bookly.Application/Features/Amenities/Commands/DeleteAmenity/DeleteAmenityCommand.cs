using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.DeleteAmenity;

public record DeleteAmenityCommand(int AmenityId) : IRequest<Result>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelAmenities];
}