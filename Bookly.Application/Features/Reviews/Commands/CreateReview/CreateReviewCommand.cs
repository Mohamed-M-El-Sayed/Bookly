using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    int BookingId,
    int Rating,
    string? Comment) : IRequest<Result<CreateReviewResponse>>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelReviews];
}