using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;

public class GetDentistDetailQueryValidator : AbstractValidator<GetDentistDetailQuery>
{
    public GetDentistDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
    }
}
