using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.CancellationPolicies.Errors;
using Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;
using Bookly.Domain.Entities.Cancellation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRuleById;

public class GetCancellationRuleByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetCancellationRuleByIdQueryHandler> logger) : IRequestHandler<GetCancellationRuleByIdQuery, Result<CancellationRulesResponse>>
{
    public async Task<Result<CancellationRulesResponse>> Handle(GetCancellationRuleByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting cancellation rule with ID {RuleId}", request.RuleId);
        var rule = await unitOfWork.Repository<CancellationRule>()
            .GetByIdAsync(request.RuleId, cancellationToken);
        if (rule is null)
        {
            logger.LogWarning("Cancellation rule {RuleId} not found", request.RuleId);
            return Result<CancellationRulesResponse>.Failure(CancellationErrors.RuleNotFound());
        }
        var response = rule.Adapt<CancellationRulesResponse>();
        logger.LogInformation("Successfully retrieved cancellation rule {RuleId}",
            rule.Id);
        return Result<CancellationRulesResponse>.Success(response);
    }
}
