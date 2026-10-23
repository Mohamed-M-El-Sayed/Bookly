using FluentValidation;

namespace Bookly.Application.Features.Amenities.Commands.UpdateAmenity;

public class UpdateAmenityCommandValidator : AbstractValidator<UpdateAmenityCommand>
{
    public UpdateAmenityCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithErrorCode("Validation.Amenity.Id.Invalid")
            .WithMessage("A valid amenity ID is required.");

        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithErrorCode("Validation.Amenity.NameEn.Required")
            .WithMessage("English name is required.")
            .MaximumLength(100)
            .WithErrorCode("Validation.Amenity.NameEn.TooLong")
            .WithMessage("English name must not exceed 100 characters.");

        RuleFor(x => x.NameAr)
            .NotEmpty()
            .WithErrorCode("Validation.Amenity.NameAr.Required")
            .WithMessage("Arabic name is required.")
            .MaximumLength(100)
            .WithErrorCode("Validation.Amenity.NameAr.TooLong")
            .WithMessage("Arabic name must not exceed 100 characters.");

        RuleFor(x => x.AmenityCategoryId)
            .GreaterThan(0)
            .WithErrorCode("Validation.Amenity.CategoryId.Invalid")
            .WithMessage("A valid amenity category ID is required.");

        RuleFor(x => x.Icon)
            .MaximumLength(100)
            .WithErrorCode("Validation.Amenity.Icon.TooLong")
            .WithMessage("Icon must not exceed 100 characters.")
            .When(x => x.Icon is not null);

        RuleFor(x => x.Scope)
            .IsInEnum()
            .WithErrorCode("Validation.Amenity.Scope.Invalid")
            .WithMessage("A valid amenity scope is required.");
    }
}