namespace Authentication.Shared.Models;

public record AuthResponse
{
    public string? AuthenticationToken { get; set; }
    public string? Errors { get; set; }
}