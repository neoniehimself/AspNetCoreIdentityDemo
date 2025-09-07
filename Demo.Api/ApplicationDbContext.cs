using Demo.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration configuration;

    public DbSet<UserProfile> UserProfiles { get; set; }

    public ApplicationDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(this.configuration["ConnectionStrings:DefaultConnection"]);
    }

    override protected void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserProfile>(u =>
        {
            u.Property(u => u.FirstName).HasMaxLength(256);
            u.Property(u => u.LastName).HasMaxLength(256);
            u.HasOne(u => u.IdentityUser).WithOne().HasForeignKey<UserProfile>(u => u.UserProfileId).OnDelete(DeleteBehavior.Cascade);
        });
        base.OnModelCreating(builder);
    }
}
