using AutomationProject.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationProject.Browser;

/// <summary>
/// Создаёт IWebDriver на основе TestConfig.Browser (Chrome, Firefox, Edge).
/// Для Firefox добавьте Selenium.WebDriver.GeckoDriver, для Edge — Selenium.WebDriver.EdgeDriver.
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
                "Firefox: добавьте пакет Selenium.WebDriver.GeckoDriver и реализуйте создание FirefoxDriver в WebDriverFactory.")
            : browser.Equals("Edge", StringComparison.OrdinalIgnoreCase)
                ? throw new NotSupportedException(
                    "Edge: добавьте пакет Selenium.WebDriver.EdgeDriver и реализуйте создание EdgeDriver в WebDriverFactory.")
                : CreateChromeDriver(settings);
    }

    private static IWebDriver CreateChromeDriver(BrowserSettings settings)
    {
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        if (settings.Headless)
            options.AddArgument("--headless=new");
        return new ChromeDriver(options);
    }

    private static void ConfigureDriver(IWebDriver driver, BrowserSettings settings)
    {
        if (settings.WindowMaximize)
            driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWaitSeconds);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(settings.PageLoadTimeoutSeconds);
    }
}
