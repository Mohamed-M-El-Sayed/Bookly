using FluentValidation;

namespace Bookly.Application.Features.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
                .WithErrorCode("Validation.Hotel.Id.Invalid")
                .WithMessage("A valid hotel id is required.");

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.Name.Required")
                .WithMessage("Hotel name is required.")
            .MaximumLength(200)
                .WithErrorCode("Validation.Hotel.Name.TooLong")
                .WithMessage("Hotel name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.Description.Required")
                .WithMessage("Description is required.")
            .MaximumLength(2000)
                .WithErrorCode("Validation.Hotel.Description.TooLong")
                .WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.DescriptionAr)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.DescriptionAr.Required")
                .WithMessage("Arabic description is required.")
            .MaximumLength(2000)
                .WithErrorCode("Validation.Hotel.DescriptionAr.TooLong")
                .WithMessage("Arabic description must not exceed 2000 characters.");

        RuleFor(x => x.StarRating)
            .InclusiveBetween(1, 5)
                .WithErrorCode("Validation.Hotel.StarRating.Invalid")
                .WithMessage("Star rating must be between 1 and 5.")
            .When(x => x.StarRating.HasValue);

        RuleFor(x => x.Street)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.Street.Required")
                .WithMessage("Street is required.")
            .MaximumLength(200)
                .WithErrorCode("Validation.Hotel.Street.TooLong")
                .WithMessage("Street must not exceed 200 characters.");

        RuleFor(x => x.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.City.Required")
                .WithMessage("City is required.")
            .MaximumLength(100)
                .WithErrorCode("Validation.Hotel.City.TooLong")
                .WithMessage("City must not exceed 100 characters.");

        RuleFor(x => x.State)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.State.Required")
                .WithMessage("State is required.")
            .MaximumLength(100)
                .WithErrorCode("Validation.Hotel.State.TooLong")
                .WithMessage("State must not exceed 100 characters.");

        RuleFor(x => x.Country)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.Country.Required")
                .WithMessage("Country is required.")
            .MaximumLength(100)
                .WithErrorCode("Validation.Hotel.Country.TooLong")
                .WithMessage("Country must not exceed 100 characters.");

        RuleFor(x => x.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.Phone.Required")
                .WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9\s\-()]{7,20}$")
                .WithErrorCode("Validation.Hotel.Phone.Invalid")
                .WithMessage("Phone number format is invalid.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithErrorCode("Validation.Hotel.Email.Required")
                .WithMessage("Email is required.")
            .EmailAddress()
                .WithErrorCode("Validation.Hotel.Email.Invalid")
                .WithMessage("Email format is invalid.");

        RuleFor(x => x.Website)
            .Must(BeAValidUrl)
                .WithErrorCode("Validation.Hotel.Website.Invalid")
                .WithMessage("Website must be a valid URL.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
    }

    private static bool BeAValidUrl(string? url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttp ||
         uri.Scheme == Uri.UriSchemeHttps);
}
