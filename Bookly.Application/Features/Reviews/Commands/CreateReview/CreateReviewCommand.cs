using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    int BookingId,
    int Rating,
    string? Comment) : IRequest<Result<CreateReviewResponse>>;
