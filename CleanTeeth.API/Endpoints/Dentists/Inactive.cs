using CleanTeeth.Application.Features.Dentists.Commands.InactiveDentist;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class Inactive : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/dentist/{id:guid}/inactive", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new InactiveDentistCommand { Id = id };
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Dentist")
        .WithName("InactiveDentist")
        .WithSummary("ChangeStatus a dentist is Inactive")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
