using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Dentists.Queries;

public sealed class DentistQueryFilter
{
    public string? Name { get; init; }
    public string? Specialty { get; init; }
    public DentistStatus? Status { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
}
