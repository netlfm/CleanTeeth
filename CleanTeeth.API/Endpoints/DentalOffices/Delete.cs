using CleanTeeth.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using MediatR;

namespace CleanTeeth.API.Endpoints.DentalOffices;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/dentaloffices/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteDentalOfficeCommand { Id = id };
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Dental Offices")
        .WithName("DeleteDentalOffice")
        .WithSummary("Delete a dental office")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
