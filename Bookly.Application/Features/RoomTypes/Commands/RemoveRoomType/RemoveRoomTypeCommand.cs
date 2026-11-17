using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Commands.RemoveRoomType;

public record RemoveRoomTypeCommand(int RoomTypeId) : IRequest<Result>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.RoomTypeAmenities];
}