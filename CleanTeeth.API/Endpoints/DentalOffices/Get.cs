using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;
using MediatR;

namespace CleanTeeth.API.Endpoints.DentalOffices;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dentaloffices", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetDentalOfficeListQuery();
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Dental Offices")
        .WithName("GetDentalOfficeList")
        .WithSummary("Get a list of dental offices")
        .Produces<List<GetDentalOfficeListResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
