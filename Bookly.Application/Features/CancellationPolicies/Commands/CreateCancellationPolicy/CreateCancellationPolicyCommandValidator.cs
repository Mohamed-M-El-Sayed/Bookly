using FluentValidation;

namespace Bookly.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;

public class CreateCancellationPolicyCommandValidator : AbstractValidator<CreateCancellationPolicyCommand>
{
    public CreateCancellationPolicyCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0)
            .WithErrorCode("validation.cancellationPolicy.hotelId.invalid");

        RuleFor(x => x.Rules)
            .NotEmpty()
            .WithErrorCode("validation.cancellationPolicy.rules.required");

        RuleForEach(x => x.Rules)
            .ChildRules(rule =>
            {
                rule.RuleFor(x => x.DaysBeforeCheckIn)
                    .GreaterThanOrEqualTo(0)
                    .WithErrorCode("validation.cancellationPolicy.daysBeforeCheckIn.invalid");

                rule.RuleFor(x => x.RefundPercentage)
                    .InclusiveBetween(0, 100)
                    .WithErrorCode("validation.cancellationPolicy.refundPercentage.invalid");
            });

        RuleFor(x => x.Rules)
            .Must(rules => rules == null ||
                rules.Select(x => x.DaysBeforeCheckIn).Distinct().Count() == rules.Count)
            .WithErrorCode("validation.cancellationPolicy.rules.duplicateThreshold");
    }
}