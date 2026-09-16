using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;

public class GetDentistDetailQuery : IRequest<GetDentistDetailResponse>
{
    public required Guid Id { get; set; }
}
