using System.Net;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace AutomationProject.Helpers;

/// <summary>
/// Copies WebDriver cookies into a CookieContainer for use with HttpClient (e.g. CartService).
/// </summary>
public static class WebDriverCookieHelper
{
    public static CookieContainer FromDriver(IWebDriver driver, string baseUrl, ILogger? logger = null)
    {
        var uri = new Uri(baseUrl.TrimEnd('/') + "/");
        var cookieContainer = new CookieContainer();

        foreach (var c in driver.Manage().Cookies.AllCookies)
        {
            try
            {
                var netCookie = new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain);
                cookieContainer.Add(uri, netCookie);
            }
            catch (CookieException ex)
            {
                logger?.LogWarning(ex, "Invalid cookie skipped: {Name}", c.Name);
            }
            catch (ArgumentException ex)
            {
                logger?.LogWarning(ex, "Invalid cookie arguments: {Name}", c.Name);
            }
        }

        return cookieContainer;
    }
}
