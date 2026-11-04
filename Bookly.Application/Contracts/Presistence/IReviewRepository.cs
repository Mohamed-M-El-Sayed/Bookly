using Bookly.Application.Features.Reviews.Queries.GetHotelReviewSummary;

namespace Bookly.Application.Contracts.Presistence;

public interface IReviewRepository
{
    Task<ReviewSummaryResponse> GetHotelReviewSummaryAsync(int hotelId, CancellationToken cancellationToken);
}
