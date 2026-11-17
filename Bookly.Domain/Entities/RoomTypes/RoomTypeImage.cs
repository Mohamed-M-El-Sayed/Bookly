using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.RoomTypes;

public class RoomTypeImage : BaseEntity
{
    public int RoomTypeId { get; private set; }
    public string Url { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    private RoomTypeImage()
    {
    }

    public RoomTypeImage(int roomTypeId, string imageUrl, bool isPrimary = false)
    {
        if (roomTypeId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roomTypeId));

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException(
                "Image URL is required.",
                nameof(imageUrl));

        RoomTypeId = roomTypeId;
        Url = imageUrl;
        IsPrimary = isPrimary;
    }

    public void SetPrimary() => IsPrimary = true;

    public void UnsetPrimary() => IsPrimary = false;
}
