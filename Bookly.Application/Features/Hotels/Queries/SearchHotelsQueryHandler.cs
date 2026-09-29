using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;

namespace Bookly.Application.Features.Hotels.Queries;

public class SearchHotelsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SearchHotelsQuery, Result<PagedResult<SearchHotelsResponse>>>
{
    public async Task<Result<PagedResult<SearchHotelsResponse>>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(
            new SearchHotelsSpecification(
                request.Location,
                request.Guests,
                request.CheckIn,
                request.CheckOut,
                request.PageNumber,
                request.PageSize
            ));
        var result = hotels.Adapt<List<SearchHotelsResponse>>();
        return Result<PagedResult<SearchHotelsResponse>>.Success(new PagedResult<SearchHotelsResponse>(result, 100, request.PageNumber, request.PageSize));
    }
}
