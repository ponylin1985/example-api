using Example.Api.Dtos.Requests;
using FluentValidation;

namespace Example.Api.Validators;

/// <summary>
/// Request validator for UpdatePatientOrderRequest.
/// </summary>
public class UpdatePatientOrderRequestValidator : AbstractValidator<UpdatePatientOrderRequest>
{
    /// <summary>
    /// Constructor for UpdatePatientOrderRequestValidator.
    /// </summary>
    public UpdatePatientOrderRequestValidator()
    {
        RuleFor(x => x.Instructions)
            .NotEmpty().WithMessage("Instructions are required.")
            .MaximumLength(500).WithMessage("Instructions cannot exceed 500 characters.")
            .WithName("instructions");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MaximumLength(50).WithMessage("UserId cannot exceed 50 characters.")
            .WithName("userId");
    }
}
