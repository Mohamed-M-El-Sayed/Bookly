using Bookly.Application.Contracts.Services.Availability;

namespace Bookly.Infrastructure.Services.BackgroundJobs.Jobs;

public class AvailabilityCleanupJob(IAvailabilityCleanupService cleanupService)
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        await cleanupService.DeleteOldDaysAsync(today,
            cancellationToken);
    }
}
