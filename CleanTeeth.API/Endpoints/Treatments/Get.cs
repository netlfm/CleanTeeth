using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;
using MediatR;

namespace CleanTeeth.API.Endpoints.Treatments;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/treatments", async (
            [AsParameters] GetTreatmentListQuery query,
            ISender sender,
            CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result);
            })
            .WithTags("Treatments")
            .WithName("GetTreatmentList")
            .WithSummary("Get a paged list of treatments")
            .Produces<PagedResult<GetTreatmentListResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
