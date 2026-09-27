using CleanTeeth.Application.Notifications;
using CleanTeeth.Infrastructure.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<INotifications, MessageService>();
        return services;
    }
}
