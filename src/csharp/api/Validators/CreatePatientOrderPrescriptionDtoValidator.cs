using Example.Api.Dtos.Requests;
using FluentValidation;
using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Request validator for CreatePatientOrderPrescriptionDto.
/// </summary>
public sealed class CreatePatientOrderPrescriptionDtoValidator : AbstractValidator<CreatePatientOrderPrescriptionDto>
{
    /// <summary>
    /// Constructor for CreatePatientOrderPrescriptionDtoValidator.
    /// </summary>
    /// <param name="sanitizer"></param>
    public CreatePatientOrderPrescriptionDtoValidator(IHtmlSanitizer sanitizer)
    {
        RuleFor(p => p.MedicationId)
            .NotNull().WithMessage("MedicationId is required.")
            .GreaterThan(0).WithMessage("MedicationId must be greater than 0.")
            .WithName("medicationId");

        RuleFor(p => p.Dose)
            .NotEmpty().WithMessage("Dose is required.")
            .MaximumLength(50).WithMessage("Dose cannot exceed 50 characters.")
            .Sanitized(sanitizer)
            .WithName("dose");

        RuleFor(p => p.Frequency)
            .NotEmpty().WithMessage("Frequency is required.")
            .MaximumLength(50).WithMessage("Frequency cannot exceed 50 characters.")
            .Sanitized(sanitizer)
            .WithName("frequency");

        RuleFor(p => p.DurationInDays)
            .NotNull().WithMessage("DurationInDays is required.")
            .GreaterThan(0).WithMessage("DurationInDays must be greater than 0.")
            .WithName("durationInDays");

        RuleFor(p => p.Route)
            .NotNull().WithMessage("Route is required.")
            .IsInEnum().WithMessage("Route must be a valid enum value.")
            .WithName("route");
    }
}
