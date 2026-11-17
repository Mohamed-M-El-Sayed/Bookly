using Bookly.Application.Contracts.Services.Availability;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Availability;

public class AvailabilityCleanupService(AppDbContext appDbContext) : IAvailabilityCleanupService
{
    public async Task<int> DeleteOldDaysAsync(DateOnly today, CancellationToken cancellationToken)
    {
        return await appDbContext.RoomAvailabilities
            .Where(x => x.Date < today)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

