using Bookly.Application.Features.Reviews.Commands.CreateReview;
using Bookly.Application.Features.Reviews.Commands.DeleteReview;
using Bookly.Application.Features.Reviews.Commands.UpdateReview;
using Bookly.Application.Features.Reviews.Queries.GetHotelReviews;
using Bookly.Application.Features.Reviews.Queries.GetHotelReviewSummary;
using Bookly.Application.Features.Reviews.Queries.GetReviewById;
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
    /// Gets paginated reviews for a hotel.
    /// </summary>
    [HttpGet("{hotelId:int}/reviews")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelReviews(int hotelId, [FromQuery] GetHotelReviewsQuery query,
        CancellationToken cancellationToken = default)
    {
        query.HotelId = hotelId;
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Gets the review summary for a hotel.
    /// </summary>
    [HttpGet("hotels/{hotelId:int}/summary")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewSummary(
        int hotelId,
        CancellationToken cancellationToken)
    {
        var query = new GetHotelReviewSummaryQuery(hotelId);
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }
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
    /// <summary>
    /// Gets a review by its ID.
    /// </summary>
    [HttpGet("{reviewId:int}")]
    public async Task<IActionResult> GetById(int reviewId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetReviewByIdQuery(reviewId), cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Deletes a review owned by the current user.
    /// Admins can delete any review.
    /// </summary>
    [HttpDelete("{reviewId:int}")]
    public async Task<IActionResult> DeleteUserReview(int reviewId, CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand(reviewId);
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result, StatusCodes.Status204NoContent);
    }
}
