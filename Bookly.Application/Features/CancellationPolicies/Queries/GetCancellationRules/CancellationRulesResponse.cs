namespace Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;

public record CancellationRulesResponse(
    int Id,
    int DaysBeforeCheckIn,
    decimal RefundPercentage);
