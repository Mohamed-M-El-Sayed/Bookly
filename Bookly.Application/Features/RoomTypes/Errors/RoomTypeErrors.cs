using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.RoomTypes.Errors;

public static class RoomTypeErrors
{
    public static Error NotFound() =>
          Application.Common.Results.ErrorFactory.NotFound("RoomType.NotFound", "Room type not found.");
}
