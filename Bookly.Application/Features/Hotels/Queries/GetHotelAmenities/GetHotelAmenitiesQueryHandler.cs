using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Queries.GetHotelAmenities;

public class GetHotelAmenitiesQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetHotelAmenitiesQueryHandler> logger) : IRequestHandler<GetHotelAmenitiesQuery, Result<IReadOnlyList<HotelAmenityResponse>>>
{
    public async Task<Result<IReadOnlyList<HotelAmenityResponse>>> Handle(GetHotelAmenitiesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting amenities for hotel {HotelId}", request.HotelId);
        var hotel = await unitOfWork.Repository<Hotel>().GetByIdAsync(request.HotelId);
        if (hotel is null)
            return Result<IReadOnlyList<HotelAmenityResponse>>.Failure(HotelErrors.NotFound());
        var hotelAmenities = await unitOfWork.Repository<HotelAmenity>()
            .GetAllWithSpecAsync(new HotelAmenitiesSpecifications(request.HotelId));
        var response = hotelAmenities.Adapt<IReadOnlyList<HotelAmenityResponse>>();
        return Result<IReadOnlyList<HotelAmenityResponse>>.Success(response);
    }
}
