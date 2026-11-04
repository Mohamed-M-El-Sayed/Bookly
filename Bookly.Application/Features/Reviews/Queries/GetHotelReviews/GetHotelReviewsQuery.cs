using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Features.Reviews.Queries.GetReviewById;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Reviews.Queries.GetHotelReviews;

public class GetHotelReviewsQuery : IRequest<Result<PagedResult<ReviewResponse>>>
{
    [JsonIgnore]
    public int HotelId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 30;
    public bool IsDescending { get; set; } = true;
}
