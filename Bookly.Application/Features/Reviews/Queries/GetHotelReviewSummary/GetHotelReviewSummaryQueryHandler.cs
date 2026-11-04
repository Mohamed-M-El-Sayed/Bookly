using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Reviews.Queries.GetHotelReviewSummary;

public class GetHotelReviewSummaryQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetHotelReviewSummaryQueryHandler> logger) : IRequestHandler<GetHotelReviewSummaryQuery, Result<ReviewSummaryResponse>>
{
    public async Task<Result<ReviewSummaryResponse>> Handle(GetHotelReviewSummaryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting review summary for hotel {HotelId}.", request.HotelId);
        var hotel = await unitOfWork.Repository<Hotel>().GetByIdAsync(request.HotelId, cancellationToken);
        if (hotel is null)
            return Result<ReviewSummaryResponse>.Failure(HotelErrors.NotFound());
        var summary = await unitOfWork.Reviews.GetHotelReviewSummaryAsync(
                    request.HotelId,
                    cancellationToken);
        logger.LogInformation("Successfully retrieved review summary for hotel {HotelId}. Total reviews: {TotalReviews}.",
           request.HotelId,
           summary.TotalReviews);
        return Result<ReviewSummaryResponse>.Success(summary);
    }

}
