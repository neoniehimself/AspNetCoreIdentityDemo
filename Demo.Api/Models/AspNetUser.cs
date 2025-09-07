using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Models;
public class AspNetUser : IdentityUser<Guid>
{
    public AspNetUser()
    {
    }

    public AspNetUser(string userName) : base(userName)
    {
    }
}
