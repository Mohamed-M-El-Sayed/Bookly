using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeAmenities;

public record GetRoomTypeAmenitiesQuery(int RoomTypeId)
    : IRequest<Result<IReadOnlyList<RoomTypeAmenityResponse>>>;

