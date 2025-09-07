using Demo.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api;
public class ApplicationDbContext(IConfiguration configuration) : IdentityDbContext<AspNetUser, AspNetRole, Guid>
{
    private readonly IConfiguration configuration = configuration;

    public DbSet<AspNetUserProfile> AspNetUserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(this.configuration["ConnectionStrings:DefaultConnection"]);
    }

    override protected void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AspNetUserProfile>(p =>
        {
            p.Property(p => p.FirstName).HasMaxLength(256);
            p.Property(p => p.LastName).HasMaxLength(256);
            p.HasOne(p => p.AspNetUser)
                .WithOne()
                .HasForeignKey<AspNetUserProfile>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
