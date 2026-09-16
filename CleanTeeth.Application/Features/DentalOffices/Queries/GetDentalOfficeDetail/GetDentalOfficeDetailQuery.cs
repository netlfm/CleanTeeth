using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;

public class GetDentalOfficeDetailQuery : IRequest<GetDentalOfficeDetailResponse>
{
    public required Guid Id { get; set; }
}
