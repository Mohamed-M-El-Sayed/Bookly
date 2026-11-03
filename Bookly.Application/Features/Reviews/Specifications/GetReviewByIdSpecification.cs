using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Reviews;

namespace Bookly.Application.Features.Reviews.Specifications;

internal class GetReviewByIdSpecification : BaseSpecification<Review>
{
    public GetReviewByIdSpecification(int reviewId)
    {
        Criteria = r => r.Id == reviewId;
        AddInclude(r => r.User);
    }
}
