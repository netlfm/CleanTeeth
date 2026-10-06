using CleanTeeth.API.Dtos.Auth;
using CleanTeeth.Application.Features.Dentists.Commands.CreateDentist;
using CleanTeeth.Security.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanTeeth.API.Endpoints.Auth;

internal sealed class RegisterDentist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register/dentist", async (
            RegisterDentistRequest request,
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
            await userManager.AddToRoleAsync(user, "Doctor");
            try
            {
                var command = new CreateDentistCommand
                {
                    UserId = user.Id,
                    Name = request.Name,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    Email = request.Email,
                    LicenseNumber = request.LicenseNumber,
                    Specialty = request.Specialty
                };
                var id = await mediator.Send(command, cancellationToken);
                return Results.CreatedAtRoute("GetDentistDetails", new { id }, id);
            }
            catch
            {
                await userManager.DeleteAsync(user);
                throw;
            }
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithName("RegisterDentist")
        .WithSummary("creates a dentist account, assigns the Doctor role and the dentist profile")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
