using Bookly.Application.Features.Amenities.Commands.Categories.CreateCategory;
using Bookly.Application.Features.Amenities.Commands.Categories.DeleteCategory;
using Bookly.Application.Features.Amenities.Commands.Categories.UpdateAmenity;
using Bookly.Application.Features.Amenities.Queries.GetCategories;
using Bookly.Application.Features.Amenities.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityCategoriesController(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Gets amenity categories.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAmenityCategoriesQuery(), cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Gets an amenity category by ID (Admin only).
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAmenityCategoryByIdQuery(id),
                cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Creates an amenity category (Admin only).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAmenityCategory([FromBody] CreateAmenityCategoryCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Updates an amenity category (Admin only).
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateAmenityCategoryCommand command,
            CancellationToken cancellationToken)
        {
            command.CategoryId = id;
            var result = await mediator.Send(command, cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Deletes an amenity category that contains no amenities (Admin only).
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            return Respond(result);
        }
    }
}
