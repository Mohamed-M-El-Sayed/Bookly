using FluentValidation;

namespace Bookly.Application.Features.Amenities.Commands.Categories.CreateCategory;

public class CreateAmenityCategoryCommandValidator
    : AbstractValidator<CreateAmenityCategoryCommand>
{
    public CreateAmenityCategoryCommandValidator()
    {
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