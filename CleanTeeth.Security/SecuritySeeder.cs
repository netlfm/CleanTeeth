using CleanTeeth.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeeth.Security;

public static class SecuritySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        string[] roles =
        {
            "Admin",
            "Dentist",
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

        // 仅当系统中还没有任何 Admin 用户时，才用配置中的凭据创建初始管理员。
        // 凭据来自 User Secrets / 环境变量，不会硬编码进源码。
        var existingAdmins = await userManager.GetUsersInRoleAsync("Admin");
        if (existingAdmins.Count > 0)
        {
            return;
        }

        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var adminUser = new User { UserName = adminEmail, Email = adminEmail };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}