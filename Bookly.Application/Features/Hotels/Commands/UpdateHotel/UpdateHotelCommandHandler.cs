using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Domain.Entities.Hotels;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateHotelCommand, Result<UpdateHotelResponse>>
{
    public async Task<Result<UpdateHotelResponse>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Repository<Hotel>().GetByIdAsync(request.Id, cancellationToken);
        if (hotel is null)
            return Result<UpdateHotelResponse>.Failure(HotelErrors.NotFound());

        var address = new Address(request.Street, request.City, request.State, request.Country);
        var contactInfo = new ContactInfo(request.Phone, request.Email, request.Website);

        hotel.Update(request.Name, request.Description, request.DescriptionAr, request.StarRating, address, contactInfo);

        unitOfWork.Repository<Hotel>().Update(hotel);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new UpdateHotelResponse(hotel.Id, hotel.Name);
        return Result<UpdateHotelResponse>.Success(response);
    }
}
