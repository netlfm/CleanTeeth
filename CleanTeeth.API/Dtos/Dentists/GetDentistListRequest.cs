using CleanTeeth.Domain.Enums;

namespace CleanTeeth.API.Dtos.Dentists;

public class GetDentistListRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? Name { get; set; }
    public string? Specialty { get; set; }
    public DentistStatus? Status { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
