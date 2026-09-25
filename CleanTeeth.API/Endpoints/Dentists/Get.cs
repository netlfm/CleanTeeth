using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dentist", async (
            [AsParameters] GetDentistListQuery query,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Dentist")
        .WithName("GetDentistList")
        .WithSummary("Get a paged list of dentist")
        .Produces<PagedResult<GetDentistListResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}