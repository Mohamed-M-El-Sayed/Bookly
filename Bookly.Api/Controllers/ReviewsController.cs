using Bookly.Application.Features.Reviews.Commands.CreateReview;
using Bookly.Application.Features.Reviews.Commands.UpdateReview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/reviews")]
[Authorize]
[ApiController]
public class ReviewsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new review for a booking.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Updates an existing review.
    /// </summary>
    [HttpPost("{reviewId:int}")]
    public async Task<IActionResult> Update([FromRoute] int reviewId, [FromBody] UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        command.ReviewId = reviewId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

}
