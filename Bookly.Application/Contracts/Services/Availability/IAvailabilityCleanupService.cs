namespace Bookly.Application.Contracts.Services.Availability;

public interface IAvailabilityCleanupService
{
    Task<int> DeleteOldDaysAsync(DateOnly today, CancellationToken cancellationToken);
}
;