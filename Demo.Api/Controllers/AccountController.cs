using Demo.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Demo.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController(
    UserManager<AspNetUser> userManager,
    RoleManager<AspNetRole> roleManager,
    IConfiguration configuration,
    ApplicationDbContext dbContext) : ControllerBase
{
    private readonly UserManager<AspNetUser> userManager = userManager;
    private readonly RoleManager<AspNetRole> roleManager = roleManager;
    private readonly IConfiguration configuration = configuration;
    private readonly ApplicationDbContext dbContext = dbContext;

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(Register register)
    {
        var result = await this.userManager.CreateAsync(new AspNetUser()
        {
            UserName = register.UserName,
            Email = register.Email
        }, register.Password);

        if (result.Succeeded)
        {
            var aspNetUser = await this.userManager.FindByNameAsync(register.UserName);
            var userProfile = new AspNetUserProfile(aspNetUser!.Id)
            {
                FirstName = register.FirstName,
                LastName = register.LastName
            };
            this.dbContext.AspNetUserProfiles.Add(userProfile);
            await this.dbContext.SaveChangesAsync();
            return Ok("Regiter successful!");
        }

        return BadRequest(result.Errors);
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
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(this.configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256
                    )
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        return Unauthorized("Invalid credentials!");
    }

    [HttpPost(nameof(AddRole))]
    public async Task<IActionResult> AddRole(string role)
    {
        if (!await this.roleManager.RoleExistsAsync(role))
        {
            var result = await this.roleManager.CreateAsync(new AspNetRole(role));
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
