using CleanTeeth.Security.Models;

namespace CleanTeeth.Security.Services;

public interface ITokenService
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
