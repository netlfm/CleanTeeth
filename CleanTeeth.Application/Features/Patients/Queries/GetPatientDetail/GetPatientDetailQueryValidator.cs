using FluentValidation;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;

public class GetPatientDetailQueryValidator : AbstractValidator<GetPatientDetailQuery>
{
    public GetPatientDetailQueryValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
