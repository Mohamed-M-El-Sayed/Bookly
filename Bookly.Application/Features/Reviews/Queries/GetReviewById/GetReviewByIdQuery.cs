using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Reviews.Queries.GetReviewById;

public record GetReviewByIdQuery(int ReviewId) : IRequest<Result<ReviewResponse>>;
