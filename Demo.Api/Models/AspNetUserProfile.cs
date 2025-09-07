using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;
/// <summary>
/// This class represents a user profile associated with an ASP.NET Identity user.
/// </summary>
public class AspNetUserProfile
{
    public string Id { get; } = null!;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public IdentityUser? IdentityUser { get; set; }

    public AspNetUserProfile()
    {
    }

    public AspNetUserProfile(string identityUserId)
    {
        this.Id = identityUserId;
    }
}
