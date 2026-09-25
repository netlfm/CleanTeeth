using CleanTeeth.API.Dtos.Appointment;
using CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;
using MediatR;

namespace CleanTeeth.API.Endpoints.Appointments;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointment", async (
            IMediator mediator,
            CreateAppointmentRequest request,
            CancellationToken cancellationtoken
            ) =>
        {
            var command = new CreateAppointmentCommand
            {
                DentalOfficeId = request.DentalOfficeId,
                DentistId = request.DentistId,
                PatientId = request.PatientId,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            var id = await mediator.Send(command, cancellationtoken);
            return Results.CreatedAtRoute("GetAppointmentDetail", new { id }, id);
        })
            .WithTags("Appointments")
            .WithName("CreateAppointment")
            .WithSummary("Create a new appointment")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
