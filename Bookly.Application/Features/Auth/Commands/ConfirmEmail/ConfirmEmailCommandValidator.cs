using FluentValidation;

namespace Bookly.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator :AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("User id is not valid.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
