using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(this.configuration["ConnectionStrings:DefaultConnection"]);
    }
}
