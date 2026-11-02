namespace Bookly.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewResponse(int Id, int HotelId, int Rating, string? Comment);
