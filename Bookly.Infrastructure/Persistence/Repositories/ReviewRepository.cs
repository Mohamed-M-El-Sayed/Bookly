using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Reviews.Queries.GetHotelReviewSummary;
using Bookly.Domain.Entities.Reviews;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Persistence.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    private readonly AppDbContext _appDbContext;
    public ReviewRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<ReviewSummaryResponse> GetHotelReviewSummaryAsync(int hotelId, CancellationToken cancellationToken)
    {
        var ratingCounts = await _appDbContext.Set<Review>().Where(r => r.HotelId == hotelId)
            .GroupBy(r => r.Rating)
            .Select(g => new
            {
                Rating = g.Key,
                Count = g.Count()
            }).ToListAsync(cancellationToken);
        var totalReviews = ratingCounts.Sum(x => x.Count);
        var averageRating = totalReviews == 0
            ? 0
            : ratingCounts.Sum(x => x.Rating * x.Count) / (double)totalReviews;
        var ratingDistribution = Enumerable.Range(1, 5)
            .ToDictionary(rating => rating,
                rating => ratingCounts.Where(x => x.Rating == rating)
                .Select(x => x.Count)
                .FirstOrDefault());
        return new(averageRating, totalReviews, ratingDistribution);
    }
}
