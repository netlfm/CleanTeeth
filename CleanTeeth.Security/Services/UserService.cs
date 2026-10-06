using CleanTeeth.Application.Contracts.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanTeeth.Security.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id != null && Guid.TryParse(id, out Guid userId) ? userId : Guid.Empty;
        }
    }
    public string? Email
    {
        get
        {
            var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            return email;
        }
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }
}
