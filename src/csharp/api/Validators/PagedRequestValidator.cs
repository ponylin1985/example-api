using Example.Api.Dtos.Requests;
using FluentValidation;

namespace Example.Api.Validators;

public sealed class PagedRequestValidator : AbstractValidator<PagedRequest>
{
    /// <summary>
    /// Constructor for PagedRequestValidator.
    /// </summary>
    public PagedRequestValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0.")
            .LessThanOrEqualTo(int.MaxValue)
            .WithName("pageNumber");

        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.")
            .WithName("pageSize");
    }
}
