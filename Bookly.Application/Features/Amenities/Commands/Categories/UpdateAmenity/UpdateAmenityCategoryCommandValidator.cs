using FluentValidation;

namespace Bookly.Application.Features.Amenities.Commands.Categories.UpdateAmenity;

public class UpdateAmenityCategoryCommandValidator
    : AbstractValidator<UpdateAmenityCategoryCommand>
{
    public UpdateAmenityCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithErrorCode("Validation.AmenityCategory.Id.Invalid");

        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithErrorCode("Validation.AmenityCategory.NameEn.Required")
            .MaximumLength(100)
            .WithErrorCode("Validation.AmenityCategory.NameEn.MaxLength");

        RuleFor(x => x.NameAr)
            .NotEmpty()
            .WithErrorCode("Validation.AmenityCategory.NameAr.Required")
            .MaximumLength(100)
            .WithErrorCode("Validation.AmenityCategory.NameAr.MaxLength");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("Validation.AmenityCategory.SortOrder.Invalid");
    }
}