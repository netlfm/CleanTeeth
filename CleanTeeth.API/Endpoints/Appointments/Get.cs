using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using MediatR;

namespace CleanTeeth.API.Endpoints.Appointments;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/appointment", async (
            IMediator mediator,
            CancellationToken cancellationToken,
            [AsParameters] GetAppointmentListQuery query) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithTags("Appointments")
        .WithName("GetAppoinementList")
        .WithSummary("Get a paged list of appoinement")
        .Produces<PagedResult<GetAppointmentListResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
