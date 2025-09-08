using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;
/// <summary>
/// This class represents the association between a user and a role in the ASP.NET Identity system.
/// </summary>
public class AspNetUserRole : IdentityUserRole<Guid>
{
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}
