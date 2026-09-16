using CleanTeeth.Application.Features.Dentists.Commands.ActiveDentist;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class Active : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/dentist/{id:guid}/active", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var dentist = new ActiveDentistCommand { Id = id };
            await mediator.Send(dentist, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Dentist")
        .WithName("ActiveDentist")
        .WithSummary("ChangeStatus a dentist is Active")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
