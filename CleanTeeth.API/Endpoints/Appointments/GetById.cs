using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDelail;
using MediatR;

namespace CleanTeeth.API.Endpoints.Appointments;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/appointment/{id:guid}", async (
                  Guid id,
                  IMediator mediator,
                  CancellationToken cancellationToken) =>
        {
            var query = new GetAppointmentDetailQuery { Id = id };
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
              .WithTags("Appointments")
              .WithName("GetAppointmentDetail")
              .WithSummary("Get appointment details by ID")
              .Produces<GetAppointmentDetailResponse>(StatusCodes.Status200OK)
              .ProducesValidationProblem(StatusCodes.Status400BadRequest)
              .ProducesProblem(StatusCodes.Status404NotFound)
              .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
