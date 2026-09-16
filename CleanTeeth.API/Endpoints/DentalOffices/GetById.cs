using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using MediatR;
namespace CleanTeeth.API.Endpoints.DentalOffices;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dentaloffices/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetDentalOfficeDetailQuery { Id = id };
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Dental Offices")
        .WithName("GetDentalOfficeDetail")
        .WithSummary("Get dental office details by ID")
        .Produces<GetDentalOfficeDetailResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
