using Example.Api.Dtos.Requests;
using FluentValidation;

namespace Example.Api.Validators;

/// <summary>
/// Request validator for GetPatientsRequest.
/// </summary>
public sealed class GetPatientsRequestValidator : AbstractValidator<GetPatientsRequest>
{
    /// <summary>
    /// Constructor for GetPatientsRequestValidator.
    /// </summary>
    public GetPatientsRequestValidator()
    {
        RuleFor(r => r.StartTime)
            .NotEmpty()
            .LessThanOrEqualTo(r => r.EndTime)
            .WithMessage("StartTime must be less than or equal to EndTime.")
            .WithName("startTime");

        RuleFor(r => r.EndTime)
            .NotEmpty()
            .GreaterThanOrEqualTo(r => r.StartTime)
            .WithMessage("EndTime must be greater than or equal to StartTime.")
            .Must((model, endTime) => endTime <= model.StartTime.AddMonths(6))
            .WithMessage("The date range must not exceed 6 months.")
            .WithName("endTime");
    }
}
