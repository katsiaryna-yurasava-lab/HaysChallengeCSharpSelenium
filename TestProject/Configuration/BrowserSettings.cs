namespace AutomationProject.Configuration;

/// <summary>
/// Browser settings for tests. Loaded from appsettings.json and environment variables.
/// </summary>
public class BrowserSettings
{
    /// <summary>Browser type: Chrome, Firefox, Edge.</summary>
    public string Browser { get; set; } = "Chrome";

    /// <summary>Run in headless mode.</summary>
    public bool Headless { get; set; }

    /// <summary>Implicit wait timeout (seconds).</summary>
    public int ImplicitWaitSeconds { get; set; } = 5;

    /// <summary>Page load timeout (seconds).</summary>
    public int PageLoadTimeoutSeconds { get; set; } = 30;

    /// <summary>Explicit wait timeout for element visibility etc. (seconds). Used by BasePage WebDriverWait.</summary>
    public int ExplicitWaitSeconds { get; set; } = 15;

    /// <summary>Maximize browser window.</summary>
    public bool WindowMaximize { get; set; } = true;
}
