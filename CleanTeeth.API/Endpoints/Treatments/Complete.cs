using CleanTeeth.API.Dtos.Treatments;
using CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;
using MediatR;

namespace CleanTeeth.API.Endpoints.Treatments;

public class Complete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/treatment/{id:guid}/complete", async (
            Guid Id,
            CompleteTreatmentRequest request,
            IMediator mediator,
            CancellationToken cancellationtoken) =>
        {
            var command = new CompleteTreatmentCommand
            {
                Id = Id,
                Notes = request.Notes
            };

            await mediator.Send(command, cancellationtoken);
            return Results.NoContent();
        })
            .WithTags("Treatments")
            .WithName("ComlpeteTreatment")
            .WithSummary("ChangeStatus a Treatment Is Complete")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
