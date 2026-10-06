using CleanTeeth.API.Dtos.Auth;
using CleanTeeth.Application.Features.Patients.Commands.CreatePatient;
using CleanTeeth.Security.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanTeeth.API.Endpoints.Auth;

internal sealed class RegisterPatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register/patient", async (
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
