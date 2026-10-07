using CleanTeeth.API.Dtos.Auth;
using CleanTeeth.Security.Models;
using CleanTeeth.Security.Services;
using Microsoft.AspNetCore.Identity;

namespace CleanTeeth.API.Endpoints.Auth;

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
                    LoginRequest request,
                    UserManager<User> userManager,
                    SignInManager<User> signInManager,
                    ITokenService tokenService) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Results.Unauthorized();
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Results.Unauthorized();
            }
            var roles = await userManager.GetRolesAsync(user);
            var token = tokenService.GenerateToken(user, roles);
            return Results.Ok(new { accessToken = token, expiresIn = "PT480M" });
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithName("Login")
        .WithSummary("Login with email and password to get a JWT access token")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
