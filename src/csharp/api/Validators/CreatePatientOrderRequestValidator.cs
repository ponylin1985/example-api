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

        RuleFor(o => o.StartDate)
            .Must(o => o >= DateTimeOffset.UtcNow)
            .When(o => o.StartDate.HasValue)
            .WithMessage("StartDate must be in the future.")
            .WithName("startDate");

        RuleFor(o => o.StartDate)
            .LessThan(o => o.EndDate)
            .When(o => o.StartDate.HasValue && o.EndDate.HasValue)
            .WithMessage("StartDate must be earlier than EndDate.")
            .WithName("startDate");

        RuleFor(o => o.EndDate)
            .Must(o => o >= DateTimeOffset.UtcNow)
            .When(o => o.EndDate.HasValue)
            .WithMessage("EndDate must be in the future.")
            .WithName("endDate");

        RuleFor(o => o.EndDate)
            .GreaterThan(o => o.StartDate)
            .When(o => o.StartDate.HasValue && o.EndDate.HasValue)
            .WithMessage("EndDate must be later than StartDate.")
            .WithName("endDate");

        RuleFor(o => o.DispensedDate)
            .Must(o => o <= DateTimeOffset.UtcNow)
            .When(o => o.DispensedDate.HasValue)
            .WithMessage("DispensedDate cannot be in the future.")
            .WithName("dispensedDate");

        RuleFor(o => o.Type)
            .NotEmpty().WithMessage("Type is required.")
            .IsInEnum().WithMessage("Type must be a valid enum value.")
            .WithName("type");

        RuleFor(o => o.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MaximumLength(50).WithMessage("UserId cannot exceed 50 characters.")
            .Sanitized(sanitizer)
            .WithName("userId");

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
