using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Reviews.Errors;
using Bookly.Domain.Entities.Reviews;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<UpdateReviewCommandHandler> logger) : IRequestHandler<UpdateReviewCommand, Result>
    {
        public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating review {ReviewId}.", request.ReviewId);
            var userId = currentUserService.UserId;
            if (userId is null)
                return Result.Failure(AuthErrors.Unauthorized());
            var review = await unitOfWork.Repository<Review>().GetByIdAsync(request.ReviewId, cancellationToken);
            if (review is null)
                return Result.Failure(ReviewErrors.NotFound());
            if (review.UserId != userId.Value)
                return Result.Failure(ReviewErrors.Forbidden());
            review.Update(request.Rating, request.Comment);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Review {ReviewId} was successfully updated by user {UserId}.",
            request.ReviewId, userId.Value);
            return Result.Success();
        }
    }
}
