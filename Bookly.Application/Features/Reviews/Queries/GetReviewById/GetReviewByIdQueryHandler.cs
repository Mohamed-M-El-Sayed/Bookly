using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Reviews.Errors;
using Bookly.Application.Features.Reviews.Specifications;
using Bookly.Domain.Entities.Reviews;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Reviews.Queries.GetReviewById;

public class GetReviewByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetReviewByIdQueryHandler> logger) : IRequestHandler<GetReviewByIdQuery, Result<ReviewResponse>>
{
    public async Task<Result<ReviewResponse>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting review {ReviewId}.",
            request.ReviewId);
        var review = await unitOfWork.Repository<Review>().GetEntityWithSpecAsync(new GetReviewByIdSpecification(request.ReviewId), cancellationToken);
        if (review is null)
            return Result<ReviewResponse>.Failure(ReviewErrors.NotFound());

        var response = new ReviewResponse(
            review.Id,
            review.ReviewerName,
            review.Rating,
            review.Comment,
            review.CreatedAt);
        return Result<ReviewResponse>.Success(response);
    }
}