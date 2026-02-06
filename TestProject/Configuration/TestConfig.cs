using Microsoft.Extensions.Configuration;

namespace AutomationProject.Configuration;

/// <summary>
/// Loads test configuration from appsettings.json and environment variables.
/// Environment variables (e.g. Browser, Headless, ImplicitWaitSeconds) override json.
/// </summary>
public static class TestConfig
{
    private static IConfiguration? _configuration;

    public static IConfiguration Configuration =>
        _configuration ??= new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

    /// <summary>
    /// Browser settings. Env: BrowserSettings__Browser, BrowserSettings__Headless, etc. override json.
    /// </summary>
    public static BrowserSettings Browser
    {
        get
        {
            var section = Configuration.GetSection("BrowserSettings");
            return section.Get<BrowserSettings>() ?? new BrowserSettings();
        }
    }

    /// <summary>
    /// Path to registered user data file. Section RegisteredUserFile in json; env: RegisteredUserFile__RelativePathFromSolution, etc.
    /// </summary>
    public static RegisteredUserFileSettings RegisteredUserFile
    {
        get
        {
            var section = Configuration.GetSection("RegisteredUserFile");
            return section.Get<RegisteredUserFileSettings>() ?? new RegisteredUserFileSettings();
        }
    }

    /// <summary>
    /// Web application settings. Section WebApp in json; env: WebApp__BaseUrl, etc.
    /// </summary>
    public static WebAppSettings WebApp
    {
        get
        {
            var section = Configuration.GetSection("WebApp");
            return section.Get<WebAppSettings>() ?? new WebAppSettings();
        }
    }

    /// <summary>
    /// Payment card data for checkout. Section PaymentCard in json; env: PaymentCard__CardNumber, etc.
    /// </summary>
    public static PaymentCardSettings PaymentCard
    {
        get
        {
            var section = Configuration.GetSection("PaymentCard");
            return section.Get<PaymentCardSettings>() ?? new PaymentCardSettings();
        }
    }
}
