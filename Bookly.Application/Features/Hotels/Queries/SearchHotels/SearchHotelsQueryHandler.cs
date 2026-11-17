using Bookly.Application.Common.Mappings;
using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Domain.Entities.Hotels;
using MediatR;

namespace Bookly.Application.Features.Hotels.Queries.SearchHotels;

public class SearchHotelsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SearchHotelsQuery, Result<PagedResult<SearchHotelsResponse>>>
{
    public async Task<Result<PagedResult<SearchHotelsResponse>>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
    {
        var specification = new SearchHotelsSpecification(
           request.Location,
           request.Guests,
           request.CheckIn,
           request.CheckOut,
           request.PageNumber,
           request.PageSize
       );
        var query = unitOfWork.Repository<Hotel>().GetQueryable(specification);
        var hotelsQuery = query.Select(h => new SearchHotelsResponse
        {
            Id = h.Id,
            Name = h.Name,
            StarRating = h.StarRating,
            City = h.Address.City,
            Country = h.Address.Country,
            PrimaryImageUrl = h.Images.Where(i => i.IsPrimary).Select(i => i.Url).FirstOrDefault().ToFullImageUrl(),
            MinPricePerNight = h.RoomTypes
            .AsQueryable()
            .Where(specification.RoomTypeFilter)
            .Min(rt => rt.BasePricePerNight)
        });
        var hotels = await unitOfWork.Repository<Hotel>().ToListAsync(hotelsQuery, cancellationToken);
        var hotelsCount = await unitOfWork.Repository<Hotel>().CountAsync(specification);
        return Result<PagedResult<SearchHotelsResponse>>.Success(new PagedResult<SearchHotelsResponse>(hotels, hotelsCount, request.PageNumber, request.PageSize));
    }
}

