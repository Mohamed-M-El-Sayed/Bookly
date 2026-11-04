using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Reviews.Errors;
using Bookly.Domain.Entities.Reviews;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler(IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<DeleteReviewCommandHandler> logger) : IRequestHandler<DeleteReviewCommand, Result>
{
    public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting review with ID {ReviewId}.", request.ReviewId);
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result.Failure(AuthErrors.Unauthorized());
        var review = await unitOfWork.Repository<Review>().GetByIdAsync(request.ReviewId, cancellationToken);
        if (review is null)
            return Result.Failure(ReviewErrors.NotFound());
        if (!currentUserService.IsAdmin && review.UserId != userId.Value)
        {
            logger.LogWarning(
                "User {UserId} attempted to delete review {ReviewId} owned by another user.",
                userId.Value, request.ReviewId);
            return Result.Failure(ReviewErrors.Forbidden());
        }
        review.Delete();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Review with ID {ReviewId} deleted successfully.", request.ReviewId);
        return Result.Success();
    }
}