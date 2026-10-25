using Bookly.Application.Contracts.Services.Availability;

namespace Bookly.Infrastructure.Services.BackgroundJobs.Jobs
{
    public class AvailabilityInitializationJob(
    IAvailabilityInitializer initializer)
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            await initializer.EnsureWindowAsync(
                today,
                cancellationToken);
        }
    }
}
