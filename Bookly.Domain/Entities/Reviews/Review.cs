using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.Identity;

namespace Bookly.Domain.Entities.Reviews
{
    public class Review : SoftDeletableEntity
    {
        public Guid UserId { get; private set; }
        public int HotelId { get; private set; }
        public int BookingId { get; private set; }

        public int Rating { get; private set; }
        public string? Comment { get; private set; }

        public ApplicationUser User { get; private set; } = null!;
        public Hotel Hotel { get; private set; } = null!;
        public Booking Booking { get; private set; } = null!;
    }
}
