using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookly.Application.Features.Users.Commands.AssignRole
{
    public class AssignRoleCommandValidator:AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required.")
                .MaximumLength(50)
                .WithMessage("Role must not exceed 50 characters.");
        }
    }
}
