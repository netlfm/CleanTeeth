using FluentValidation;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;

public class GetAppointmentListValidator : AbstractValidator<GetAppointmentListQuery>
{
    public GetAppointmentListValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The field {PropertyName} must be greater than or equal to 1.");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The field {PropertyName} must be greater than or equal to 1.");
    }
}
