using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Security.Models;
using CleanTeeth.Security.Options;
using CleanTeeth.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanTeeth.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddSecurityService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<CleanTeethSecurityDbContext>(options =>
            options.UseSqlServer("name=CleanTeethConnectionString"));
        service.AddIdentityCore<User>(options =>
        {
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<CleanTeethSecurityDbContext>()
            .AddSignInManager();
        service.AddAuthorizationBuilder()
            .AddPolicy("Doctor", policy =>
                policy.RequireRole("Doctor", "Admin"))
            .AddPolicy("Patient", policy =>
                policy.RequireRole("Patient", "Admin"))
            .AddPolicy("Admin", policy =>
                policy.RequireRole("Admin"));

        service.AddAuthorization();

        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException($"Missing configuration section '{JwtOptions.SectionName}'.");
        if (string.IsNullOrWhiteSpace(jwtOptions.SigningKey))
        {
            throw new InvalidOperationException("Missing 'Jwt:SigningKey'. Store it in User Secrets.");
        }

        service.AddSingleton(jwtOptions);
        service.AddScoped<ITokenService, TokenService>();

        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        service.AddHttpContextAccessor();
        service.AddTransient<IUserService, UserService>();
        return service;
    }
}