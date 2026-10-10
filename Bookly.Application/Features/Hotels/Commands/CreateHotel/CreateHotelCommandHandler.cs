using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Domain.Entities.Hotels;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.CreateHotel;

public class CreateHotelCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateHotelCommand, Result<CreateHotelResponse>>
{
    public async Task<Result<CreateHotelResponse>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.State, request.Country);
        var contactInfo = new ContactInfo(request.Phone, request.Email, request.Website);

        var hotel = Hotel.Create(
            request.Name,
            request.Description,
            "gg",
            request.StarRating,
            address,
            contactInfo);

        await unitOfWork.Repository<Hotel>().AddAsync(hotel, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateHotelResponse(hotel.Id, hotel.Name);
        return Result<CreateHotelResponse>.Success(response);
    }
}
