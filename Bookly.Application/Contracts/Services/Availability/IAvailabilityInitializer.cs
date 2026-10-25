namespace Bookly.Application.Contracts.Services.Availability
{
    public interface IAvailabilityInitializer
    {
        Task<int> EnsureWindowAsync(DateOnly today, CancellationToken cancellationToken);

        Task<int> EnsureWindowForRoomTypeAsync(int roomTypeId, DateOnly today, CancellationToken cancellationToken);
    }
}
