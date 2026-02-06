using Microsoft.Extensions.Configuration;

namespace AutomationProject.Configuration;

/// <summary>
/// Загрузка конфигурации тестов из appsettings.json и переменных окружения.
/// Переменные окружения: Browser, Headless, ImplicitWaitSeconds и т.д. (переопределяют json).
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
    /// Настройки браузера. Переменные окружения BrowserSettings__Browser, BrowserSettings__Headless и т.д. переопределяют json.
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
    /// Путь к файлу с данными пользователя. Секция RegisteredUserFile в json; env: RegisteredUserFile__RelativePathFromSolution и т.д.
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
    /// Настройки веб-приложения. Секция WebApp в json; env: WebApp__BaseUrl и т.д.
    /// </summary>
    public static WebAppSettings WebApp
    {
        get
        {
            var section = Configuration.GetSection("WebApp");
            return section.Get<WebAppSettings>() ?? new WebAppSettings();
        }
    }
}
