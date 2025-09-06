using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "User")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("User access granted.");
    }
}
