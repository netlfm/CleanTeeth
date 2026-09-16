using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Persistence.Repositories;

public class DentalOfficeRepository : Repository<DentalOffice>, IDentalOfficeRepository
{
    public DentalOfficeRepository(CleanTeethDbContext context)
        : base(context)
    {

    }
}
