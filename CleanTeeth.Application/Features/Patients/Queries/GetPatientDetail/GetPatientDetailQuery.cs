using MediatR;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;

public class GetPatientDetailQuery : IRequest<GetPatientDetailResponse>
{
    public required Guid Id { get; set; }
}
