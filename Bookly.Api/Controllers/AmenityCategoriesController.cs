using Bookly.Application.Features.AmenityCategories.Commands.ActivateCategory;
using Bookly.Application.Features.AmenityCategories.Commands.CreateCategory;
using Bookly.Application.Features.AmenityCategories.Commands.DeactivateCategory;
using Bookly.Application.Features.AmenityCategories.Commands.DeleteCategory;
using Bookly.Application.Features.AmenityCategories.Commands.UpdateCategory;
using Bookly.Application.Features.AmenityCategories.Queries.GetCategories;
using Bookly.Application.Features.AmenityCategories.Queries.GetCategoryById;
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
        /// <summary>
        /// Activates an amenity category (Admin only).
        /// </summary>
        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> ActivateCategory([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ActivateAmenityCategoryCommand(id), cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Deactivates an amenity category (Admin only).
        /// </summary>
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> DeactivateCategory([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeactivateAmenityCategoryCommand(id), cancellationToken);
            return Respond(result);
        }
    }
}
