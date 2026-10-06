using FluentValidation;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;

public class GetTreatmentDetailQueryValidator : AbstractValidator<GetTreatmentDetailResponse>
{
    public GetTreatmentDetailQueryValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
