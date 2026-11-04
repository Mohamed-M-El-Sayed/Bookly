using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Reviews.Queries.GetHotelReviewSummary;

public record GetHotelReviewSummaryQuery(int HotelId) : IRequest<Result<ReviewSummaryResponse>>;