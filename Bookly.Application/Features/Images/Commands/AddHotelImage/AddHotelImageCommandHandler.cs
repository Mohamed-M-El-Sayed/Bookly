using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.HotelImages.Specifications;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using MediatR;

namespace Bookly.Application.Features.HotelImages.Commands.AddHotelImage;

public class AddHotelImageCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService) : IRequestHandler<AddHotelImageCommand, Result<AddHotelImageResponse>>
{
    public async Task<Result<AddHotelImageResponse>> Handle(AddHotelImageCommand request, CancellationToken cancellationToken)
    {
        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(request.HotelId, cancellationToken);
        if (!accessResult.IsSuccess)
            return Result<AddHotelImageResponse>.Failure(accessResult.Error!);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetEntityWithSpecAsync(
            new HotelByIdWithImagesSpecification(request.HotelId), cancellationToken);
        if (hotel is null)
            return Result<AddHotelImageResponse>.Failure(HotelErrors.NotFound());
        hotel.AddImage(new HotelImage(request.HotelId, request.Url, request.IsPrimary));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<AddHotelImageResponse>.Success(new AddHotelImageResponse(request.HotelId, request.Url, request.IsPrimary));
    }
}
