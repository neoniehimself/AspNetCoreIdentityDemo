using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;
public class AspNetRole : IdentityRole<Guid>
{
    public AspNetRole()
    {
    }

    public AspNetRole(string roleName) : base(roleName)
    {
    }
}
