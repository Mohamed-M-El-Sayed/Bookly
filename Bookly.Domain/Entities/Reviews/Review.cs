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
        public string ReviewerName => $"{User?.FirstName} {User?.LastName}" ?? "Unknown";
        public ApplicationUser User { get; private set; } = null!;
        public Hotel Hotel { get; private set; } = null!;
        public Booking Booking { get; private set; } = null!;

        private Review() { }

        private Review(Guid userId, int hotelId, int bookingId, int rating, string? comment)
        {
            UserId = userId;
            HotelId = hotelId;
            BookingId = bookingId;
            Rating = rating;
            Comment = comment;
        }

        public static Review Create(Guid userId, int hotelId, int bookingId, int rating, string? comment)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID must not be empty.", nameof(userId));

            if (hotelId <= 0)
                throw new ArgumentException("Hotel ID must be greater than 0.", nameof(hotelId));

            if (bookingId <= 0)
                throw new ArgumentException("Booking ID must be greater than 0.", nameof(bookingId));

            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), rating, "Rating must be between 1 and 5.");

            return new Review(userId, hotelId, bookingId, rating, comment?.Trim());
        }
        public void Update(int rating, string? comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), rating, "Rating must be between 1 and 5.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment is required.");

            Rating = rating;
            Comment = comment?.Trim();
        }
    }
}
