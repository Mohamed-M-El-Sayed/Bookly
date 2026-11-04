namespace Bookly.Application.Features.Reviews.Queries.GetHotelReviewSummary;

public record ReviewSummaryResponse(
    double AverageRating,
    int TotalReviews,
    Dictionary<int, int> RatingDistribution);

