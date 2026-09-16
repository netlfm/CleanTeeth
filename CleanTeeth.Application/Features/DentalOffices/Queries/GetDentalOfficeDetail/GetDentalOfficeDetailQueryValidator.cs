using FluentValidation;

namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;

public class GetDentalOfficeDetailQueryValidator : AbstractValidator<GetDentalOfficeDetailQuery>
{
    public GetDentalOfficeDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
    }
}
