using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Bookings
{
    public class BookingRoom : BaseEntity
    {
        public int BookingId { get; private set; }
        public int RoomTypeId { get; private set; }
        public int? BoardOptionId { get; private set; }
        public int? RoomId { get; private set; }
        public decimal PriceAtBooking { get; private set; }
        private BookingRoom() { }

    }
}
