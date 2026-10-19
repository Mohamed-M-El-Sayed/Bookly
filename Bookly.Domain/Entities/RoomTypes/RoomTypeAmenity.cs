using Bookly.Domain.Entities.Amenities;

namespace Bookly.Domain.Entities.RoomTypes;

public class RoomTypeAmenity
{
    public int AmenityId { get; private set; }
    public int RoomTypeId { get; private set; }
    public Amenity Amenity { get; private set; } = default!;
    public RoomType RoomType { get; private set; } = default!;
    private RoomTypeAmenity() { }
    public RoomTypeAmenity(int amenityId, int roomTypeId)
    {
        AmenityId = amenityId;
        RoomTypeId = roomTypeId;
    }
}
