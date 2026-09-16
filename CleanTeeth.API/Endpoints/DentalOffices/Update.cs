using CleanTeeth.API.Dtos.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using MediatR;
namespace CleanTeeth.API.Endpoints.DentalOffices;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/dentaloffices/{id:guid}", async (
            Guid id,
            UpdateDentalOfficeRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateDentalOfficeCommand
            {
                Id = id,
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email
            };

            await mediator.Send(command, cancellationToken);

            return Results.NoContent();
        })
            .WithTags("Dental Offices")
            .WithName("UpdateDentalOffice")
            .WithSummary("Update an existing dental office")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
