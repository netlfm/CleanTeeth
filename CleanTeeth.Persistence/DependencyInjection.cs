using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Persistence.Repositories;
using CleanTeeth.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<CleanTeethDbContext>(option => option.UseSqlServer("name=CleanTeethConnectionString"));
        services.AddScoped<IDentalOfficeRepository, DentalOfficeRepository>();
        services.AddScoped<IDentistRepository, DentistRepository>();
        //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWorkEFCore>();
        return services;
    }
}
