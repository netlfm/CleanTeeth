using CleanTeeth.API.Dtos.Dentists;
using CleanTeeth.Application.Features.Dentists.Commands.UpdateDentist;
using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

public class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/dentist/{id:guid}", async (
            IMediator mediator,
            Guid id,
            UpdateDentistRequest request,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateDentistCommand
            {
                Id = id,
                Name = request.Name,
                Gender = request.Gender,
                Email = request.Email,
                Phone = request.Phone,
                LicenseNumber = request.LicenseNumber,
                Specialty = request.Specialty
            };
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithTags("Dentist")
        .WithName("UpdateDentist")
        .WithSummary("Update an existing dentist")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
