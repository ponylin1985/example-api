using Example.Api.Dtos.Requests;
using FluentValidation;

namespace Example.Api.Validators;

/// <summary>
/// Request validator for UpdateOrderInstructionseRequest.
/// </summary>
public class UpdateOrderInstructionseRequestValidator : AbstractValidator<UpdateOrderInstructionseRequest>
{
    /// <summary>
    /// Constructor for UpdateOrderInstructionseRequestValidator.
    /// </summary>
    public UpdateOrderInstructionseRequestValidator()
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
