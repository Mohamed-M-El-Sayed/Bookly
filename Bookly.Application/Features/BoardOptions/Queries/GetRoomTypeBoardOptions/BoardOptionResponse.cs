using Bookly.Domain.Enums;

namespace Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;

public record BoardOptionResponse(
    int Id,
    int RoomTypeId,
    BoardType Type,
    bool IsActive,
    decimal ExtraPricePerNight);
