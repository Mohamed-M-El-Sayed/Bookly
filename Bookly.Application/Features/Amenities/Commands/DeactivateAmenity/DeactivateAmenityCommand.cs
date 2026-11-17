using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.DeactivateAmenity;

public record DeactivateAmenityCommand(int Id) : IRequest<Result>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelAmenities];
}