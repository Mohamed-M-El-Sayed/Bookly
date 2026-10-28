using Bookly.Application.Features.Bookings.Commands.ExpireBooking;
using MediatR;

namespace Bookly.Infrastructure.Services.BackgroundJobs.Jobs
{
    public class BookingExpirationJob(IMediator mediator)
    {
        public async Task ExecuteAsync(int bookingId)
        {
            await mediator.Send(new ExpireBookingCommand(bookingId));
        }
    }
}
