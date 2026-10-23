namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeAmenities;

public record RoomTypeAmenityResponse(
    int AmenityId,
    string NameEn,
    string NameAr,
    string Icon,
    bool IsActive,
    int CategoryId,
    string CategoryNameEn,
    string CategoryNameAr);
