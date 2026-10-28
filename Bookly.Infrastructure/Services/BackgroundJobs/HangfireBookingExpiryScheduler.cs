using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Infrastructure.Services.BackgroundJobs.Jobs;
using Hangfire;

namespace Bookly.Infrastructure.Services.BackgroundJobs;

public class HangfireBookingExpiryScheduler(IBackgroundJobClient backgroundJobClient) : IBookingExpiryScheduler
{
    public Task ScheduleAsync(int bookingId, TimeSpan delay, CancellationToken cancellationToken)
    {
        backgroundJobClient.Schedule<BookingExpirationJob>(job => job.ExecuteAsync(bookingId), delay);
        return Task.CompletedTask;
    }
}