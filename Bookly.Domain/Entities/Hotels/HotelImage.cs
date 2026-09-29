using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Hotels;

public class HotelImage : BaseEntity
{
    public int HotelId { get; private set; }
    public string Url { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    private HotelImage()
    {
    }

    public HotelImage(int hotelId, string imageUrl, bool isPrimary = false)
    {
        if (hotelId <= 0)
            throw new ArgumentOutOfRangeException(nameof(hotelId));

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException(
                "Image URL is required.",
                nameof(imageUrl));

        HotelId = hotelId;
        Url = imageUrl;
        IsPrimary = isPrimary;
    }

    public void SetPrimary() => IsPrimary = true;

    public void UnsetPrimary() => IsPrimary = false;
}