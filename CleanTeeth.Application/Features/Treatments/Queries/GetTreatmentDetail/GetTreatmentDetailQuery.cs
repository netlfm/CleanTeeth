using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;

public class GetTreatmentDetailQuery : IRequest<GetTreatmentDetailResponse>
{
    public required Guid Id { get; set; }
}
