using CleanTeeth.Security.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Security;

public class CleanTeethSecurityDbContext : IdentityDbContext<User, Role, Guid>
{
    public CleanTeethSecurityDbContext(DbContextOptions<CleanTeethSecurityDbContext> options) : base(options)
    {

    }
    protected CleanTeethSecurityDbContext()
    {

    }
}