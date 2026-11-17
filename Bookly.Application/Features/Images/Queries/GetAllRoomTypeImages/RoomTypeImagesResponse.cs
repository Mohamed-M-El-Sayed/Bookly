namespace Bookly.Application.Features.Images.Queries.GetAllRoomTypeImages;

public record RoomTypeImagesResponse(int Id, int RoomTypeId, string Url, bool IsPrimary);
