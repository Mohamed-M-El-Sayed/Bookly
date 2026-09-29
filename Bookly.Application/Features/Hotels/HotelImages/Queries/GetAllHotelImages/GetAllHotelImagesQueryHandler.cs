using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.HotelImages.Specifications;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;

namespace Bookly.Application.Features.Hotels.HotelImages.Queries.GetAllHotelImages;

public class GetAllHotelImagesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllHotelImagesQuery, Result<List<GetAllHotelImagesResponse>>>
{
    public async Task<Result<List<GetAllHotelImagesResponse>>> Handle(GetAllHotelImagesQuery request, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
            return Result<List<GetAllHotelImagesResponse>>
                .Failure(Errors.NotFound("Hotel not found"));


        var images = await unitOfWork.Repository<HotelImage>()
            .GetAllWithSpecAsync(
                new HotelImagesByHotelIdSpecification(request.HotelId),
                cancellationToken);

        var response = images.Adapt<List<GetAllHotelImagesResponse>>();

        return Result<List<GetAllHotelImagesResponse>>
            .Success(response);
    }
}
