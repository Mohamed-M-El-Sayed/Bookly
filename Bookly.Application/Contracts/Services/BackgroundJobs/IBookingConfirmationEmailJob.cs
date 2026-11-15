namespace Bookly.Application.Contracts.Services.BackgroundJobs;

public interface IBookingConfirmationEmailJob
{
    Task SendAsync(int bookingId, CancellationToken cancellationToken);

}
