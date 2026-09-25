using CleanTeeth.Application.Features.Appointments.Commands.CompleteAppointment;
using MediatR;

namespace CleanTeeth.API.Endpoints.Appointments;

public class Complete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointment/{id:guid}/complete", async (
           Guid id,
           IMediator mediator,
           CancellationToken cancellationToken) =>
        {
            var appointment = new CompleteAppointmentCommand { Id = id };
            await mediator.Send(appointment, cancellationToken);
            return Results.NoContent();
        })
       .WithTags("Appointments")
       .WithName("CompleteAppointment")
       .WithSummary("ChangeStatus a Appointment is Complete")
       .Produces<Guid>(StatusCodes.Status201Created)
       .ProducesValidationProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
