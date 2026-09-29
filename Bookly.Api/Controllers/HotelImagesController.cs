using Bookly.Api.Extensions;
using Bookly.Application.Features.Hotels.HotelImages.Commands.AddHotelImage;
using Bookly.Application.Features.Hotels.HotelImages.Queries.GetAllHotelImages;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers
{
    [Route("api/hotels/{hotelId}/images")]
    [ApiController]
    public class HotelImagesController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Adds a new image to the specified hotel.
        /// </summary>
        /// <param name="hotelId">The hotel ID</param>
        /// <param name="command">The image details to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added hotel image details</returns>
        [HttpPost]
        public async Task<IActionResult> Create(int hotelId, [FromBody] AddHotelImageCommand command, CancellationToken cancellationToken)
        {
            command.HotelId = hotelId;
            var result = await mediator.Send(command, cancellationToken);
            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves all images for the specified hotel.
        /// </summary>
        /// <param name="hotelId">The hotel ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of all hotel images</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int hotelId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllHotelImagesQuery(hotelId), cancellationToken);
            return result.ToResponse();
        }
    }
}
