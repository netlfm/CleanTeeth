using CleanTeeth.API.Dtos.Dentists;
using CleanTeeth.Application.Features.Dentists.Commands.CreateDentist;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/dentist", async (
            CreateDentistRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateDentistCommand
            {
                Name = request.Name,
                Gender = request.Gender,
                Email = request.Email,
                Phone = request.Phone,
                LicenseNumber = request.LicenseNumber,
                Specialty = request.Specialty
            };
            var id = await mediator.Send(command, cancellationToken);
            return Results.CreatedAtRoute("GetDentistDetails", new { id }, id);
        })
            .WithTags("Dentist")
            .WithName("CreateDentist")
            .WithSummary("Create a new dentist")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
