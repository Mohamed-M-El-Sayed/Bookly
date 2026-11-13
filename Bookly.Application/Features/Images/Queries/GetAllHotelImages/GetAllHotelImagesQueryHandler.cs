using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.HotelImages.Specifications;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;

namespace Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;

public class GetAllHotelImagesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllHotelImagesQuery, Result<List<HotelImagesResponse>>>
{
    public async Task<Result<List<HotelImagesResponse>>> Handle(GetAllHotelImagesQuery request, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
            return Result<List<HotelImagesResponse>>
                .Failure(HotelErrors.NotFound());


        var images = await unitOfWork.Repository<HotelImage>()
            .GetAllWithSpecAsync(
                new HotelImagesByHotelIdSpecification(request.HotelId),
                cancellationToken);

        var response = images.Adapt<List<HotelImagesResponse>>();

        return Result<List<HotelImagesResponse>>
            .Success(response);
    }
}
