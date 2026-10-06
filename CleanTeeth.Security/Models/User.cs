using Microsoft.AspNetCore.Identity;

namespace CleanTeeth.Security.Models;

public class User : IdentityUser<Guid>
{
    public DateTime CreateTime { get; private set; } = DateTime.UtcNow;
}
