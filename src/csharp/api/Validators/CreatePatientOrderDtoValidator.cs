using Example.Api.Dtos.Requests;
using FluentValidation;
using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Request validator for CreatePatientOrderDto.
/// </summary>
public sealed class CreatePatientOrderDtoValidator : AbstractValidator<CreatePatientOrderDto>
{
    /// <summary>
    /// Constructor for CreatePatientOrderDtoValidator.
    /// </summary>
    /// <param name="sanitizer"></param>
    public CreatePatientOrderDtoValidator(IHtmlSanitizer sanitizer)
    {
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
                prescriptionRule.SetValidator(new CreatePatientOrderPrescriptionDtoValidator(sanitizer));
            })
            .WithName("prescriptions");
    }
}
