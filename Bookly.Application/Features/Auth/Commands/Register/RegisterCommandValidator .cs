using FluentValidation;

namespace Bookly.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.FirstName.Required")
                .WithMessage("First name is required.")
            .MaximumLength(50)
                .WithErrorCode("Validation.FirstName.TooLong")
                .WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.LastName.Required")
                .WithMessage("Last name is required.")
            .MaximumLength(50)
                .WithErrorCode("Validation.LastName.TooLong")
                .WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Email.Required")
                .WithMessage("Email is required.")
            .EmailAddress()
                .WithErrorCode("Validation.Email.Invalid")
                .WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Password.Required")
                .WithMessage("Password is required.")
            .MinimumLength(8)
                .WithErrorCode("Validation.Password.TooShort")
                .WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.ConfirmPassword.Required")
                .WithMessage("Confirm password is required.")
            .Equal(x => x.Password)
                .WithErrorCode("Validation.ConfirmPassword.Mismatch")
                .WithMessage("Passwords do not match.");
    }
}
