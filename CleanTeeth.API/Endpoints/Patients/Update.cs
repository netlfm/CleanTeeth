using CleanTeeth.API.Dtos.Patients;
using CleanTeeth.Application.Features.Patients.Commands.UpdatePatient;
using MediatR;

namespace CleanTeeth.API.Endpoints.Patients
{
    public class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/patient/{id:guid}", async (
                IMediator mediator,
                Guid id,
                UpdatePatientRequest request,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePatientCommand
                {
                    Id = id,
                    Name = request.Name,
                    Gender = request.Gender,
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address
                };
                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .WithTags("Patient")
            .WithName("UpdatePatient")
            .WithSummary("Update an existing patient")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
