using CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;
using MediatR;

namespace CleanTeeth.API.Endpoints.Patients;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/patient/{id:guid}", async (
               Guid id,
               IMediator mediator,
               CancellationToken cancellationToken) =>
        {
            var dentist = new GetPatientDetailQuery { Id = id };
            var result = await mediator.Send(dentist, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Patient")
        .WithName("GetPatientDetails")
        .WithSummary("Get patient details by ID")
        .Produces<GetPatientDetailResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
