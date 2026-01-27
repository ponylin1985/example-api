using Example.Api.Dtos.Requests;
using FluentValidation;
using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Request validator for CreatePatientRequest.
/// </summary>
public sealed class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
{
    /// <summary>
    /// Constructor for CreatePatientRequestValidator.
    /// </summary>
    /// <param name="sanitizer"></param>
    public CreatePatientRequestValidator(IHtmlSanitizer sanitizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
            .Sanitized(sanitizer)
            .WithName("name");

        RuleFor(x => x.Age)
            .NotNull().WithMessage("Age is required.")
            .InclusiveBetween(1, 150)
            .WithName("age");

        RuleFor(x => x.Gender)
            .NotNull().WithMessage("Gender is required.")
            .IsInEnum().WithMessage("Gender must be a valid enum value.")
            .WithName("gender");

        RuleFor(x => x.Email)
            .MaximumLength(100)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithName("email");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .MaximumLength(10)
            .Matches(@"^$|^[0-9]+$").WithMessage("PhoneNumber must be numeric.")
            .Sanitized(sanitizer)
            .WithName("phoneNumber");

        RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("DateOfBirth is required.")
            .InclusiveBetween(new DateOnly(1900, 1, 1), new DateOnly(2100, 12, 31))
            .WithName("dateOfBirth");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MaximumLength(50).WithMessage("UserId cannot exceed 50 characters.")
            .Sanitized(sanitizer)
            .WithName("userId");

        RuleFor(x => x.Order)
            .NotNull().WithMessage("Order is required.")
            .SetValidator(new CreatePatientOrderDtoValidator(sanitizer) as IValidator<CreatePatientOrderDto?>)
            .WithName("orders");
    }
}
