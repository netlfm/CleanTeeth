using CleanTeeth.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Security;

public static class SecuritySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<Role>>();
        string[] roles =
        {
            "Admin",
            "Doctor",
            "Patient"
        };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = role
                });
            }
        }
    }
}