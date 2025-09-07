using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;

public class UserProfile
{
    public string UserProfileId { get; } = null!;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public IdentityUser? IdentityUser { get; set; }

    public UserProfile()
    {
    }

    public UserProfile(string id)
    {
        this.UserProfileId = id;
    }
}
