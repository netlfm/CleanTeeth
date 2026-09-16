using CleanTeeth.API.Dtos.DentalOffices;
using CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using MediatR;

namespace CleanTeeth.API.Endpoints.DentalOffices;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/dentaloffices", async (
            CreateDentalOfficeRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateDentalOfficeCommand
            {
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email
            };

            var id = await mediator.Send(command, cancellationToken);
            return Results.CreatedAtRoute(
                            "GetDentalOfficeDetail",
                            new { id },
                            id);
        })
            .WithTags("Dental Offices")
            .WithName("CreateDentalOffice")
            .WithSummary("Create a new dental office")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}