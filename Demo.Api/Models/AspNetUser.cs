using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;
/// <summary>
/// This class represents an ASP.NET Core Identity User with a GUID as the primary key.
/// </summary>
public class AspNetUser : IdentityUser<Guid>
{
    public AspNetUser()
    {
    }

    public AspNetUser(string userName) : base(userName)
    {
    }
}
