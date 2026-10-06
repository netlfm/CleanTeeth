namespace CleanTeeth.Application.Contracts.Security;

public interface IUserService
{
    public Guid UserId { get; }
    public string? Email { get; }
    bool IsInRole(string role);
}
