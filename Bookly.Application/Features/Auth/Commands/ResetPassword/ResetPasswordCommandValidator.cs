using FluentValidation;

namespace Bookly.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
             .NotEmpty().WithMessage("User id is required.")
             .Must(id => Guid.TryParse(id, out _)).WithMessage("User id is not valid.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain a digit.");
        }
    }
}
