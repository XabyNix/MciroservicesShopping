namespace Authentication.Shared.Models;

public record LoginRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}