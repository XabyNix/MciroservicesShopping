namespace Authentication.Shared.Settings;

public class Settings
{
    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;

    public Settings(IConfiguration configuration, IServiceCollection services)
    {
        _configuration = configuration;
        _services = services;
    }

    public T Configure<T>(string sectionName) where T : class
    {
        var section = _configuration.GetSection(sectionName);
        var sectionFields = section.Get<T>();
        _services.Configure<T>(section);
        return sectionFields;
    }
}