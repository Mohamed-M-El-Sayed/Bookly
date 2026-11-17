using Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;

namespace Bookly.Application.Features.RoomTypes.Queries.GetAvailableHotelRoomTypes;

public record AvailableRoomTypeResponse(
  int Id,
  string NameEn,
  string NameAr,
  string Description,
  string DescriptionAr,
  string View,
  string ViewAr,
  decimal SizeSqm,
  int MaxOccupancy,
  int BedCount,
  decimal Price,
  IReadOnlyList<RoomTypeImageResponse> Images,
  IReadOnlyList<BoardOptionResponse> BoardOptions);

public record RoomTypeImageResponse(
    int Id,
    string Url,
    bool IsPrimary
);