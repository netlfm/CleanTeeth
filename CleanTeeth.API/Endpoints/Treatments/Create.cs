using CleanTeeth.API.Dtos.Treatments;
using CleanTeeth.Application.Features.Treatments.Command.CreateTreatment;
using MediatR;

namespace CleanTeeth.API.Endpoints.Treatments;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/treatment", async (
            CreateTreatmentRequest request,
            IMediator mediator,
            CancellationToken cancellationtoken) =>
        {
            var command = new CreateTreatmentCommand { AppointmentId = request.AppointmentId };
            var id = await mediator.Send(command, cancellationtoken);
            return Results.CreatedAtRoute("GetTreatmentDetails", new { id }, id);

        })
            .WithTags("Treatments")
            .WithName("CreateTreatment")
            .WithSummary("Create a new Treatment")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
