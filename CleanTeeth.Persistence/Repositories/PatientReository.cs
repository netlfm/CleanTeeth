using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Persistence.Repositories;

public class PatientReository : Repository<Patient>, IPatientRepository
{
    public PatientReository(CleanTeethDbContext context) : base(context)
    {
    }
}
