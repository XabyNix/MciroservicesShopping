namespace Authentication.Shared.Settings;

public class JwtSettings
{
    public string PublicKeys { get; set; }
    public string Authority { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecurityKey { get; set; }
}