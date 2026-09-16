using CleanTeeth.Application.Features.Dentists.Commands.DeleteDentist;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/dentist/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteDentistCommand { Id = id };
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Dentist")
        .WithName("DeleteDentist")
        .WithSummary("Delete a dentist")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
