using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;

public record GetCancellationRulesQuery(int HotelId) : IRequest<Result<IReadOnlyList<CancellationRulesResponse>>>;
