using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;

public class GetDentalOfficeListQuery : IRequest<List<GetDentalOfficeListResponse>>
{
}
