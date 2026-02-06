using System.Collections.Generic;
using AutomationProject.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationProject.Browser;

/// <summary>
/// Creates IWebDriver based on TestConfig.Browser (Chrome, Firefox, Edge).
/// For Firefox add Selenium.WebDriver.GeckoDriver, for Edge add Selenium.WebDriver.EdgeDriver.
/// </summary>
public class WebDriverFactory : IWebDriverFactory
{
    public IWebDriver Create()
    {
        var settings = TestConfig.Browser;
        var driver = CreateDriver(settings);
        ConfigureDriver(driver, settings);
        return driver;
    }

    private static IWebDriver CreateDriver(BrowserSettings settings)
    {
        var browser = settings.Browser.Trim();

        return browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase)
            ? throw new NotSupportedException(
                "Firefox: add Selenium.WebDriver.GeckoDriver package and implement FirefoxDriver creation in WebDriverFactory.")
            : browser.Equals("Edge", StringComparison.OrdinalIgnoreCase)
                ? throw new NotSupportedException(
                    "Edge: add Selenium.WebDriver.EdgeDriver package and implement EdgeDriver creation in WebDriverFactory.")
                : CreateChromeDriver(settings);
    }

    private static IWebDriver CreateChromeDriver(BrowserSettings settings)
    {
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-extensions");

        options.AddUserProfilePreference("profile.default_content_setting_values.ads", 2);
        options.AddUserProfilePreference("profile.default_content_setting_values.popups", 2);

        if (settings.Headless)
            options.AddArgument("--headless=new");

        var driver = new ChromeDriver(options);

        driver.ExecuteCdpCommand("Network.enable", new Dictionary<string, object>());
        driver.ExecuteCdpCommand("Network.setBlockedURLs", new Dictionary<string, object>
        {
            ["urls"] = new[]
            {
                "*doubleclick.net*",
                "*googlesyndication.com*",
                "*googleadservices.com*"
            }
        });

        return driver;
    }

    private static void ConfigureDriver(IWebDriver driver, BrowserSettings settings)
    {
        if (settings.WindowMaximize)
            driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWaitSeconds);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(settings.PageLoadTimeoutSeconds);
    }
}
