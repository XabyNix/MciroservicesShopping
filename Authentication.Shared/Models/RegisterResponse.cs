namespace Authentication.Shared.Models;

public record RegisterResponse
{
    public bool Succeded { get; set; }
    public IEnumerable<string> Errors { get; set; }
}