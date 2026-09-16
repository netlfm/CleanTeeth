using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.UpdateDentist;

public class UpdateDentistCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Gender Gender { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialty { get; set; }
}
