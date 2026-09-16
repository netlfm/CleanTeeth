using CleanTeeth.API.Dtos.Patients;
using CleanTeeth.Application.Features.Patients.Commands.CreatePatient;
using FluentValidation;
using MediatR;

namespace CleanTeeth.API.Endpoints.Patients;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/patient", async (
            CreatePatientRequest request,
            IMediator mediator,
            CancellationToken cancellationtoken) =>
        {

            var command = new CreatePatientCommand
            {
                PatientNumber = request.PatientNumber,
                Name = request.Name,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address
            };
            var id = await mediator.Send(command, cancellationtoken);
            //return Results.CreatedAtRoute("", new { id }, id);
            return Results.Created($"/api/patient/{id}", id);
        })
            .WithTags("Patient")
            .WithName("CreatePatient")
            .WithSummary("Create a new patient")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
