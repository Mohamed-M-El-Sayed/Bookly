namespace Bookly.Application.Features.Reviews.Queries.GetReviewById;

public record ReviewResponse(
 int Id,
 string ReviewerName,
 int Rating,
 string? Comment,
 DateTime CreatedAt);
