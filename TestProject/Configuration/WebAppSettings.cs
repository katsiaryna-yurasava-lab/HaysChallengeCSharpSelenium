namespace AutomationProject.Configuration;

/// <summary>
/// Настройки тестируемого веб-приложения.
/// </summary>
public class WebAppSettings
{
    /// <summary>Базовый URL приложения (без завершающего слэша).</summary>
    public string BaseUrl { get; set; } = "https://automationexercise.com";
}
