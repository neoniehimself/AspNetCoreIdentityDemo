namespace Demo.Api.Models;
/// <summary>
/// This class represents a user profile associated with an ASP.NET Identity user.
/// </summary>
public class AspNetUserProfile
{
    public Guid Id { get; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public AspNetUser? AspNetUser { get; set; }

    public AspNetUserProfile()
    {
    }

    public AspNetUserProfile(Guid aspNetUserId)
    {
        this.Id = aspNetUserId;
    }
}
