using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Cancellation;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;

public class GetCancellationRulesQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetCancellationRulesQueryHandler> logger) : IRequestHandler<GetCancellationRulesQuery, Result<IReadOnlyList<CancellationRulesResponse>>>
{
    public async Task<Result<IReadOnlyList<CancellationRulesResponse>>> Handle(GetCancellationRulesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching cancellation rules for hotel {HotelId}", request.HotelId);
        var hotel = await unitOfWork.Repository<Hotel>().GetByIdAsync(request.HotelId, cancellationToken);
        if (hotel is null)
        {
            logger.LogWarning("Hotel {HotelId} not found when fetching cancellation rules", request.HotelId);
            return Result<IReadOnlyList<CancellationRulesResponse>>.Failure(HotelErrors.NotFound());
        }
        var rules = await unitOfWork.Repository<CancellationRule>()
            .GetAllWithSpecAsync(
                new CancellationRulesByHotelSpecification(request.HotelId),
                cancellationToken);

        var response = rules.Adapt<List<CancellationRulesResponse>>().AsReadOnly();

        logger.LogInformation("Retrieved {Count} cancellation rules for hotel {HotelId}",
            response.Count, request.HotelId);
        return Result<IReadOnlyList<CancellationRulesResponse>>.Success(response);
    }
}
