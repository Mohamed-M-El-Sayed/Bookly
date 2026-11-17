using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using Bookly.Application.Features.RoomTypes.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Queries.GetHotelRoomTypes;

public class GetHotelRoomTypesQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<GetHotelRoomTypesQueryHandler> logger)
    : IRequestHandler<GetHotelRoomTypesQuery, Result<IReadOnlyList<RoomTypeResponse>>>
{
    public async Task<Result<IReadOnlyList<RoomTypeResponse>>> Handle(GetHotelRoomTypesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving room types for hotel {HotelId}.",
            request.HotelId);

        await hotelAccessService.EnsureCurrentUserManagesAsync(
            request.HotelId,
            cancellationToken);

        var roomTypes = await unitOfWork.Repository<RoomType>()
            .GetAllWithSpecAsync(
                new HotelRoomTypesSpecification(request.HotelId),
                cancellationToken);

        var response = roomTypes.Adapt<IReadOnlyList<RoomTypeResponse>>();

        logger.LogInformation(
            "Retrieved {Count} room types for hotel {HotelId}.",
            response.Count,
            request.HotelId);

        return Result<IReadOnlyList<RoomTypeResponse>>.Success(response);
    }
}