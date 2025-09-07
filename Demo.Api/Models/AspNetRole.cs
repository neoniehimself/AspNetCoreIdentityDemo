using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;
/// <summary>
/// This class represents an ASP.NET Core Identity Role with a GUID as the primary key.
/// </summary>
public class AspNetRole : IdentityRole<Guid>
{
    public AspNetRole()
    {
    }

    public AspNetRole(string roleName) : base(roleName)
    {
    }
}
