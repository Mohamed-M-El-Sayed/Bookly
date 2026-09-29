using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.HotelImages.Specifications;
using Bookly.Domain.Entities.Hotels;
using MediatR;

namespace Bookly.Application.Features.Hotels.HotelImages.Commands.AddHotelImage;

public class AddHotelImageCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddHotelImageCommand, Result<AddHotelImageResponse>>
{
    public async Task<Result<AddHotelImageResponse>> Handle(AddHotelImageCommand request, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Repository<Hotel>()
            .GetEntityWithSpecAsync(
            new HotelByIdWithImagesSpecification(request.HotelId), cancellationToken);
        if (hotel is null)
            return Result<AddHotelImageResponse>.Failure(Errors.NotFound("Hotel not found"));
        hotel.AddImage(new HotelImage(request.HotelId, request.Url, request.IsPrimary));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<AddHotelImageResponse>.Success(new AddHotelImageResponse(request.HotelId, request.Url, request.IsPrimary));
    }
}
