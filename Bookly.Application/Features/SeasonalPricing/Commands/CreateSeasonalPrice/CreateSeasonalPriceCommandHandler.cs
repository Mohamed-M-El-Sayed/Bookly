using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.SeasonalPricing.Commands.CreateSeasonalPrice;
using Bookly.Application.Features.SeasonalPricing.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Domain.Entities.SeasonalPricing;
using MediatR;

public class CreateSeasonalPriceCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService)
    : IRequestHandler<CreateSeasonalPriceCommand, Result<CreateSeasonalPriceResponse>>
{
    public async Task<Result<CreateSeasonalPriceResponse>> Handle(CreateSeasonalPriceCommand request,
        CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(new RoomTypeWithSeasonalPricesSpecification(request.RoomTypeId),
                cancellationToken);

        if (roomType is null)
            return Result<CreateSeasonalPriceResponse>.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result<CreateSeasonalPriceResponse>.Failure(accessResult.Error!);

        var seasonalPrice = new SeasonalPrice(
            request.RoomTypeId,
            request.Name,
            request.StartDate,
            request.EndDate,
            request.PricePerNight);

        roomType.AddSeasonalPrice(seasonalPrice);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateSeasonalPriceResponse>.Success(
            new CreateSeasonalPriceResponse(seasonalPrice.Id, seasonalPrice.Name));
    }
}