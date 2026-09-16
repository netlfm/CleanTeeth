using FluentValidation;
namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;

public class GetDentistListQueryValidator : AbstractValidator<GetDentistListQuery>
{
    public GetDentistListQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The field {PropertyName} must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The field {PropertyName} must be greater than or equal to 1.");
    }
}