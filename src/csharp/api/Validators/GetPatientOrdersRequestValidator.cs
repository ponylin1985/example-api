using FluentValidation;
using Example.Api.Dtos.Requests;

namespace Example.Api.Validators;

/// <summary>
/// Validator for GetPatientOrdersRequest
/// </summary>
public sealed class GetPatientOrdersRequestValidator : AbstractValidator<GetPatientOrdersRequest>
{
    /// <summary>
    /// Constructor for GetPatientOrdersRequestValidator.
    /// </summary>
    public GetPatientOrdersRequestValidator()
    {
        RuleFor(r => r.PatientId)
            .GreaterThan(0)
            .When(r => r.PatientId.HasValue)
            .WithMessage("PatientId is required.")
            .WithName("patientId");

        RuleFor(r => r.Type)
            .IsInEnum()
            .When(r => r.Type.HasValue)
            .WithMessage("OrderType is invalid.")
            .WithName("orderType");

        RuleFor(r => r.Status)
            .IsInEnum()
            .When(r => r.Status.HasValue)
            .WithMessage("Status is invalid.")
            .WithName("status");
    }
}
