using OpenQA.Selenium;

namespace AutomationProject.Browser;

/// <summary>
/// Factory for creating IWebDriver instances. Browser type and options are set via configuration.
/// </summary>
public interface IWebDriverFactory
{
    /// <summary>
    /// Creates and configures the driver according to configuration (Browser, Headless, timeouts, etc.).
    /// </summary>
    IWebDriver Create();
}
