using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Amenities.Commands.ActivateAmenity;

public record ActivateAmenityCommand(int Id) : IRequest<Result>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelAmenities, CacheTags.RoomTypeAmenities];
}