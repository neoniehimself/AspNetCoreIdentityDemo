using Demo.Api.Models;
using Demo.Api.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Demo.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration configuration;

    public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.configuration = configuration;
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(Register register)
    {
        var user = new IdentityUser
        {
            UserName = register.UserName,
            Email = register.Email
        };

        var result = await this.userManager.CreateAsync(user, register.Password);
        return result.Succeeded ? Ok("Register successful!") : BadRequest(result.Errors);
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(Login login)
    {
        var user = await this.userManager.FindByNameAsync(login.UserName);

        if (user != null && await this.userManager.CheckPasswordAsync(user, login.Password))
        {
            var userRoles = await this.userManager.GetRolesAsync(user);

            if (userRoles.Count == 0)
            {
                return Unauthorized("User has no roles assigned!");
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken
            (
                issuer: this.configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256
                    )
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        return Unauthorized("Invalid credentials!");
    }

    [HttpPost(nameof(AddRole))]
    public async Task<IActionResult> AddRole(string role)
    {
        if (!await this.roleManager.RoleExistsAsync(role))
        {
            var result = await this.roleManager.CreateAsync(new IdentityRole(role));
            return result.Succeeded ? Ok("Role created successfully!") : BadRequest(result.Errors);
        }
        return BadRequest("Role already exists!");
    }

    [HttpPost(nameof(AssignRole))]
    public async Task<IActionResult> AssignRole(UserRole userRole)
    {
        var user = await this.userManager.FindByNameAsync(userRole.UserName);

        if (user == null)
        {
            return NotFound("User not found!");
        }

        if (!await this.roleManager.RoleExistsAsync(userRole.Role))
        {
            return NotFound("Role not found!");
        }

        if (await this.userManager.IsInRoleAsync(user, userRole.Role))
        {
            return BadRequest("User already assigned to this role!");
        }

        var result = await this.userManager.AddToRoleAsync(user, userRole.Role);
        return result.Succeeded ? Ok("Role assigned successfully!") : BadRequest(result.Errors);
    }
}
