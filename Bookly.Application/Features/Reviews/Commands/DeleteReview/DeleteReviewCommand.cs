using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(int ReviewId) : IRequest<Result>;

