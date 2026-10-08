using CleanTeeth.API.Dtos.Auth;
using CleanTeeth.Application.Features.Patients.Commands.CreatePatient;
using CleanTeeth.Security.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CleanTeeth.API.Endpoints.Auth;

internal sealed class RegisterPatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register/patient", async (
            HttpContext httpContext,
            RegisterPatientRequest request,
            UserManager<User> userManager,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var user = new User { UserName = request.Email, Email = request.Email };
            var createResult = await userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return Results.BadRequest(createResult.Errors);
            }
            await userManager.AddToRoleAsync(user, "Patient");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Role, "Patient")
            };
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));

            try
            {
                var command = new CreatePatientCommand
                {
                    UserId = user.Id,
                    Name = request.Name,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address = request.Address
                };
                var id = await mediator.Send(command, cancellationToken);
                return Results.CreatedAtRoute("GetPatientDetails", new { id }, id);
            }
            catch
            {
                await userManager.DeleteAsync(user);
                throw;
            }
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithName("RegisterPatient")
        .WithSummary("creates the account, assigns the Patient role and the patient profile")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
