using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Reviews.Errors;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.Reviews;
using Bookly.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<CreateReviewCommandHandler> logger) : IRequestHandler<CreateReviewCommand, Result<CreateReviewResponse>>
{
    public async Task<Result<CreateReviewResponse>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
        {
            logger.LogWarning("Review creation attempted by unauthenticated user");
            return Result<CreateReviewResponse>.Failure(AuthErrors.Unauthorized());
        }

        logger.LogInformation("Attempting to create review for booking {BookingId} by user {UserId}",
            request.BookingId, userId);

        var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(request.BookingId, cancellationToken);
        if (booking is null)
        {
            logger.LogWarning("Review creation failed: Booking {BookingId} not found", request.BookingId);
            return Result<CreateReviewResponse>.Failure(ReviewErrors.BookingNotFound());
        }

        if (booking.UserId != userId.Value)
        {
            logger.LogWarning("Review creation failed: User {UserId} attempted to review booking {BookingId} that belongs to user {BookingUserId}",
                userId, request.BookingId, booking.UserId);
            return Result<CreateReviewResponse>.Failure(ReviewErrors.UnauthorizedBooking());
        }
        if (booking.Status != BookingStatus.Completed)
        {
            return Result<CreateReviewResponse>.Failure(ReviewErrors.BookingNotCompleted());
        }

        var review = Review.Create(
            userId.Value,
            booking.HotelId,
            request.BookingId,
            request.Rating,
            request.Comment);

        await unitOfWork.Repository<Review>().AddAsync(review, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Review successfully created with ID {ReviewId} for hotel {HotelId} by user {UserId}",
            review.Id, review.HotelId, userId);

        var response = new CreateReviewResponse(review.Id, review.HotelId, review.Rating, review.Comment);
        return Result<CreateReviewResponse>.Success(response);
    }
}