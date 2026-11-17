using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;
using Bookly.Application.Features.RoomTypes.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Queries.GetAvailableHotelRoomTypes;

public class GetAvailableHotelRoomTypesQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetAvailableHotelRoomTypesQueryHandler> logger) : IRequestHandler<GetAvailableHotelRoomTypesQuery,
    Result<PagedResult<AvailableRoomTypeResponse>>>
{
    public async Task<Result<PagedResult<AvailableRoomTypeResponse>>> Handle(GetAvailableHotelRoomTypesQuery request, CancellationToken cancellationToken)
    {
        var specififcation = new AvailableRoomTypesSpecification(request.HotelId,
            request.CheckIn, request.CheckOut, request.Guests, request.PageNumer, request.PageSize);
        var roomTypes = await unitOfWork.Repository<RoomType>().GetAllWithSpecAsync(specififcation, cancellationToken);
        var totalCount = await unitOfWork.Repository<RoomType>().CountAsync(specififcation, cancellationToken);
        var response = roomTypes
             .Select(rt => new AvailableRoomTypeResponse(
                 rt.Id,
                 rt.NameEn,
                 rt.NameAr,
                 rt.Description,
                 rt.DescriptionAr,
                 rt.View,
                 rt.ViewAr,
                 rt.SizeSqm,
                 rt.MaxOccupancy,
                 rt.BedCount,
                 rt.TotalPriceForStay(request.CheckIn, request.CheckOut, null),
                 rt.Images.Adapt<IReadOnlyList<RoomTypeImageResponse>>(),
                 rt.BoardOptions.Adapt<IReadOnlyList<BoardOptionResponse>>()
                 )).ToList();

        return Result<PagedResult<AvailableRoomTypeResponse>>.Success(
            new PagedResult<AvailableRoomTypeResponse>(response, totalCount, request.PageNumer, request.PageSize));
    }
}
