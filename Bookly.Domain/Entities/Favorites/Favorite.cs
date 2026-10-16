using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.Identity;

namespace Bookly.Domain.Entities.Favorites;

public class Favorite : BaseEntity
{
    public Guid UserId { get; private set; }
    public int HotelId { get; private set; }
    public ApplicationUser User { get; private set; } = null!;
    public Hotel Hotel { get; private set; } = null!;
    public Favorite(Guid userId, int hotelId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User ID is required.", nameof(userId));

        if (hotelId <= 0)
            throw new ArgumentOutOfRangeException(nameof(hotelId));
        UserId = userId;
        HotelId = hotelId;
    }


}
