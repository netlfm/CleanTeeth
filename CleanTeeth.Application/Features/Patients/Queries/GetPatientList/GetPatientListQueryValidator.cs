using FluentValidation;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientList;

public class GetPatientListQueryValidator : AbstractValidator<GetPatientListQuery>
{
    public GetPatientListQueryValidator()
    {
        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The field {PropertyName} must be greater than or equal to 1.");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The field {PropertyName} must be greater than or equal to 1.");

    }
}
