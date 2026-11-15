using FluentValidation;

namespace Bookly.Application.Features.CancellationPolicies.Commands.UpdateCancellationRule;

public class UpdateCancellationRuleCommandValidator : AbstractValidator<UpdateCancellationRuleCommand>
{
    public UpdateCancellationRuleCommandValidator()
    {
        RuleFor(x => x.DaysBeforeCheckIn)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("validation.cancellation.invalidDays");

        RuleFor(x => x.RefundPercentage)
            .InclusiveBetween(0, 100)
            .WithErrorCode("validation.cancellation.invalidRefund");
    }
}
