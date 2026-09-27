using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;
using MediatR;

namespace CleanTeeth.API.Endpoints.Treatments;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/treatment/{id:guid}", async (
               Guid id,
               IMediator mediator,
               CancellationToken cancellationToken) =>
        {
            var dentist = new GetTreatmentDetailQuery { Id = id };
            var result = await mediator.Send(dentist, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Treatments")
        .WithName("GetTreatmentDetails")
        .WithSummary("Get Treatment details by ID")
        .Produces<GetTreatmentDetailResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
