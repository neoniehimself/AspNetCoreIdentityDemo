namespace Demo.Api.Models;
/// <summary>
/// This class represents a login request with username and password.
/// </summary>
public class Login
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
