namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public record RoomTypeResponse(
    int Id,
    int HotelId,
    string NameEn,
    string NameAr,
    decimal BasePricePerNight,
    int MaxOccupancy,
    string Description,
    string DescriptionAr,
    string View,
    string ViewAr,
    int BedCount,
    decimal SizeSqm);