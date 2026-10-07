using CleanTeeth.Application.Features.Treatments.Command.CancelTreatment;
using MediatR;

namespace CleanTeeth.API.Endpoints.Treatments;

public class Cancel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/treatment/{id:guid}/cancel", async (
            Guid Id,
            IMediator mediator,
            CancellationToken cancellationtoken) =>
        {
            var command = new CancelTreatmentCommand { Id = Id };
            await mediator.Send(command, cancellationtoken);
            return Results.NoContent();
        })
            .WithTags("Treatments")
            .WithName("CancelTreatment")
            .WithSummary("ChangeStatus a Treatment Is Cancel")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("Dentist");
    }
}
