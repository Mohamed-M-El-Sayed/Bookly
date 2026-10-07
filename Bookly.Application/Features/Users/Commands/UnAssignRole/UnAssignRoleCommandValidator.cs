using FluentValidation;

namespace Bookly.Application.Features.Users.Commands.UnAssignRole
{
    public class UnAssignRoleCommandValidator:AbstractValidator<UnAssignRoleCommand>
    {
        public UnAssignRoleCommandValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required.")
                .MaximumLength(50)
                .WithMessage("Role must not exceed 50 characters.");
        }
    }
}
