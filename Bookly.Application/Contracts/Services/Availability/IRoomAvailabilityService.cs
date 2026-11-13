using Bookly.Application.Common.Results;

namespace Bookly.Application.Contracts.Services.Availability;

public interface IRoomAvailabilityService
{
    Task<Result> SyncRoomTypeAsync(int roomTypeId, DateOnly today, CancellationToken cancellationToken);
}
