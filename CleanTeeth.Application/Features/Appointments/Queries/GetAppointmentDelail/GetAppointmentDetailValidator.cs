using FluentValidation;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDelail;

public class GetAppointmentDetailValidator : AbstractValidator<GetAppointmentDetailQuery>
{
    public GetAppointmentDetailValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
