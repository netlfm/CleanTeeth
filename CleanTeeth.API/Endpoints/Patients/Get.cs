using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using MediatR;

namespace CleanTeeth.API.Endpoints.Patients;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/patient", async (
            IMediator mediator,
            CancellationToken cancellationToken,
            [AsParameters] GetPatientListQuery query) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Patient")
        .WithName("GetPatientList")
        .WithSummary("Get a paged list of patient")
        .Produces<PagedResult<GetPatientListResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
