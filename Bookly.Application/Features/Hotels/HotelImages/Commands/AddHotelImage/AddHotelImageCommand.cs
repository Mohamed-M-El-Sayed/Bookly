using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.HotelImages.Commands.AddHotelImage
{
    public class AddHotelImageCommand : IRequest<Result<AddHotelImageResponse>>
    {
        public int HotelId { get; set; }
        public string Url { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}
