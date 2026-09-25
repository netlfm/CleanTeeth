using CleanTeeth.Application.Features.Appointments.Commands.CancelAppointment;
using MediatR;

namespace CleanTeeth.API.Endpoints.Appointments;

public class Cancel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointment/{id:guid}/cancel", async (
           Guid id,
           IMediator mediator,
           CancellationToken cancellationToken) =>
        {
            var appointment = new CancelAppointmentCommand { Id = id };
            await mediator.Send(appointment, cancellationToken);
            return Results.NoContent();
        })
       .WithTags("Appointments")
       .WithName("CancelAppointment")
       .WithSummary("ChangeStatus a Appointment is Cancel")
       .Produces<Guid>(StatusCodes.Status201Created)
       .ProducesValidationProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
