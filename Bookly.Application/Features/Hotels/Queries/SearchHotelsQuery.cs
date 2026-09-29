using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Queries;

public class SearchHotelsQuery : IRequest<Result<PagedResult<SearchHotelsResponse>>>
{
    public string Location { get; set; } = default!;
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int Guests { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
