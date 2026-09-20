using CleanTeeth.Application.Features.Patients.Commands.DeletePatient;
using MediatR;

namespace CleanTeeth.API.Endpoints.Patients;

internal sealed class Delete:IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/patient/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new DeletePatientCommand { Id = id };
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Patient")
        .WithName("DeletePatient")
        .WithSummary("Delete a patient")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
