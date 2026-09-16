using CleanTeeth.Application.Features.Dentists.Commands.OnLeaveDentist;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class OnLeave : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/dentist/{id:guid}/onleave", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new OnLeaveDentistCommand { Id = id };
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Dentist")
        .WithName("OnLeaveDentist")
        .WithSummary("ChangeStatus a dentist is OnLeave")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
