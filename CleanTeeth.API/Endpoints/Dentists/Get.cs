using CleanTeeth.API.Dtos.Dentists;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using MediatR;

namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dentist", async (
            IMediator mediator,
            CancellationToken cancellationToken,
            [AsParameters] GetDentistListRequest request) =>
        {
            var query = new GetDentistListQuery
            {
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                Name = request.Name,
                Specialty = request.Specialty,
                Status = request.Status,
                CreatedFrom = request.CreatedFrom,
                CreatedTo = request.CreatedTo
            };
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Dentist")
        .WithName("GetDentistList")
        .WithSummary("Get a paged list of dentist")
        .Produces<PagedResult<GetDentistListReponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}