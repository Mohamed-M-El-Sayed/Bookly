using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Reviews.Queries.GetReviewById;
using Bookly.Application.Features.Reviews.Specifications;
using Bookly.Domain.Entities.Reviews;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Reviews.Queries.GetHotelReviews;

public class GetHotelReviewsQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetHotelReviewsQueryHandler> logger) : IRequestHandler<GetHotelReviewsQuery, Result<PagedResult<ReviewResponse>>>
{
    public async Task<Result<PagedResult<ReviewResponse>>> Handle(GetHotelReviewsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting reviews for hotel {HotelId}. ", request.HotelId);

        var specification = new HotelReviewsSpecification(
            request.HotelId,
            request.PageNumber,
            request.PageSize,
            request.IsDescending);
        var reviews = await unitOfWork.Repository<Review>()
            .GetAllWithSpecAsync(specification, cancellationToken);
        var totalCount = await unitOfWork.Repository<Review>().CountAsync(specification, cancellationToken);
        var reviewResponses = reviews.Adapt<List<ReviewResponse>>();
        var result = new PagedResult<ReviewResponse>(
            reviewResponses,
            totalCount,
            request.PageNumber,
            request.PageSize);
        logger.LogInformation(
            "Retrieved {ReviewCount} reviews for hotel {HotelId}.",
            reviewResponses.Count,
            request.HotelId);
        return Result<PagedResult<ReviewResponse>>.Success(result);
    }
}
