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
    public CreatePatientOrderPrescriptionDtoValidator()
    {
        RuleFor(p => p.MedicationId)
            .NotNull().WithMessage("MedicationId is required.")
            .GreaterThan(0).WithMessage("MedicationId must be greater than 0.")
            .WithName("medicationId");

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
