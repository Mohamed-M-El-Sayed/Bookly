namespace Bookly.Application.Contracts.Services.BackgroundJobs
{
    public interface IBookingExpiryScheduler
    {
        Task ScheduleAsync(int bookingId, TimeSpan delay, CancellationToken cancellationToken);
    }
}
