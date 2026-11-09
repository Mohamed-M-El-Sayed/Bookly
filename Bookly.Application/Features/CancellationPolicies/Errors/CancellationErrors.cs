using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.CancellationPolicies.Errors;

public static class CancellationErrors
{
    public static Error PolicyNotFound() =>
        ErrorFactory.NotFound("cancellation.policy.notFound", "Cancellation policy was not found.");

    public static Error RuleNotFound =
        ErrorFactory.NotFound("cancellation.rule.notFound", "Cancellation rule was not found.");

    public static Error ThresholdAlreadyExists =
        ErrorFactory.Conflict("cancellation.rule.thresholdAlreadyExists",
            "A cancellation rule with the same days-before-check-in value already exists.");
}
