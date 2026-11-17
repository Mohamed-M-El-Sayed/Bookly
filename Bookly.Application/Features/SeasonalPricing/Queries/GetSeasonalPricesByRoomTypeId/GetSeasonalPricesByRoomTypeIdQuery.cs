using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPriceById;
using MediatR;

namespace Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPricesByRoomTypeId;

public record GetSeasonalPricesByRoomTypeIdQuery(int RoomTypeId,
 int PageNumber,
 int PageSize) : IRequest<Result<PagedResult<SeasonalPriceResponse>>>;
