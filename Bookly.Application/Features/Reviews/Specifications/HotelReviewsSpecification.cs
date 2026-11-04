using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Reviews;

namespace Bookly.Application.Features.Reviews.Specifications;

internal class HotelReviewsSpecification : BaseSpecification<Review>
{
    public HotelReviewsSpecification(int hotelId, int pageNumber, int pageSize, bool isDescending)
    {
        Criteria = r => r.HotelId == hotelId;
        AddInclude(r => r.User);
        ApplyPagination(pageNumber, pageSize);
        if (isDescending)
            ApplyOrderByDescending(r => r.Rating);
        ApplyOrderBy(r => r.Rating);
    }
}
