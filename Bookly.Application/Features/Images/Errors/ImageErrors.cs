using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Images.Errors
{
    public static class ImageErrors
    {
        public static Error HotelImageNotFound() =>
            ErrorFactory.NotFound("Image.NotFound", "Image not found.");
        public static Error RoomTypeImageNotFound() =>
            ErrorFactory.NotFound("Image.NotFound", "Image not found.");
    }
}
