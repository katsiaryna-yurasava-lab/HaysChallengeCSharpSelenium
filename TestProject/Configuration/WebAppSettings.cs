namespace AutomationProject.Configuration;

/// <summary>
/// Settings for the web application under test.
/// </summary>
public class WebAppSettings
{
    /// <summary>Base URL of the application (no trailing slash).</summary>
    public string BaseUrl { get; set; } = "https://automationexercise.com";
}
