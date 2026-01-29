using Example.Api.Dtos.Requests;
using FluentValidation;
using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Validator for CreatePatientOrderRequest.
/// </summary>
public sealed class CreatePatientOrderRequestValidator : AbstractValidator<CreatePatientOrderRequest>
{
    /// <summary>
    /// Constructor for CreatePatientOrderRequestValidator.
    /// </summary>
    /// <param name="sanitizer"></param>
    public CreatePatientOrderRequestValidator(IHtmlSanitizer sanitizer)
    {
        RuleFor(x => x.PatientId)
            .NotNull().WithMessage("PatientId is required.")
            .NotEmpty().WithMessage("PatientId is required.")
            .GreaterThan(0).WithMessage("PatientId must be greater than 0.");

        RuleFor(o => o.Instructions)
            .MaximumLength(500).WithMessage("Instructions cannot exceed 500 characters.")
            .Sanitized(sanitizer)
            .WithName("instructions");

        RuleFor(o => o.Type)
            .NotEmpty().WithMessage("Type is required.")
            .IsInEnum().WithMessage("Type must be a valid enum value.")
            .WithName("type");

        RuleFor(o => o.Prescriptions)
            .NotNull().WithMessage("Prescriptions are required.")
            .NotEmpty().WithMessage("At least one prescription is required.")
            .ForEach(prescriptionRule =>
            {
                prescriptionRule.NotNull().WithMessage("Prescription item cannot be null.");
                prescriptionRule.SetValidator(new CreatePatientOrderPrescriptionDtoValidator());
            })
            .WithName("prescriptions");
    }
}
