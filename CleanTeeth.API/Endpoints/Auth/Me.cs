using CleanTeeth.Security.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CleanTeeth.API.Endpoints.Auth;

public class Me : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", async (
            UserManager<User> userManager,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var currentUser = await userManager.FindByIdAsync(userId);
            if (currentUser is null)
            {
                return Results.NotFound();
            }
            var roles = await userManager.GetRolesAsync(currentUser);
            return Results.Ok(new
            {
                UserId = userId,
                Email = currentUser.Email,
                Roles = roles
            });
        })
        .RequireAuthorization()
        .WithTags("Auth")
        .WithSummary("Get current user information");
    }
}
