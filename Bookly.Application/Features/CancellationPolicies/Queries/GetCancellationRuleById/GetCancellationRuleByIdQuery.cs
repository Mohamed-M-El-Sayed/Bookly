using Bookly.Application.Common.Results;
using Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;
using MediatR;

namespace Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRuleById;

public record GetCancellationRuleByIdQuery(int RuleId) : IRequest<Result<CancellationRulesResponse>>;
