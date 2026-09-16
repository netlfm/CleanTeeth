using CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;
using MediatR;
namespace CleanTeeth.API.Endpoints.Dentists;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dentist/{id:guid}", async (
               Guid id,
               IMediator mediator,
               CancellationToken cancellationToken) =>

        {
            var dentist = new GetDentistDetailQuery { Id = id };
            var result = await mediator.Send(dentist, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Dentist")
        .WithName("GetDentistDetails")
        .WithSummary("Get dentist details by ID")
        .Produces<GetDentistDetailResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
