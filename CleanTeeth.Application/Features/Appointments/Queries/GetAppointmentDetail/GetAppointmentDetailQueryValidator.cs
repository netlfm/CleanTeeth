using FluentValidation;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDetail;

public class GetAppointmentDetailQueryValidator : AbstractValidator<GetAppointmentDetailQuery>
{
    public GetAppointmentDetailQueryValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
