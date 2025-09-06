using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=.;Database=AspNetCoreIdentityDemo;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    }
}
