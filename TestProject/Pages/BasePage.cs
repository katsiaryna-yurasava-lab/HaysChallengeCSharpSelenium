using AutomationProject.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationProject.Pages;

public abstract class BasePage
{
    protected static string BaseUrl => TestConfig.WebApp.BaseUrl.TrimEnd('/');
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver, int? timeoutSeconds = null)
    {
        Driver = driver;
        var seconds = timeoutSeconds ?? TestConfig.Browser.ExplicitWaitSeconds;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
    }

    protected IWebElement WaitAndFind(By locator) =>
        Wait.Until(ExpectedConditions.ElementExists(locator));

    protected IWebElement WaitVisible(By locator) =>
        Wait.Until(ExpectedConditions.ElementIsVisible(locator));

    protected bool IsVisible(By locator)
    {
        try
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator)).Displayed;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Closes consent popup if present (e.g. "This site asks for consent to use your data").
    /// </summary>
    protected void AcceptConsentIfPresent()
    {
        try
        {
            var consent = Driver.FindElements(By.CssSelector("button[aria-label='Consent']"));
            if (consent.Count > 0 && consent[0].Displayed)
            {
                consent[0].Click();
            }
        }
        catch
        {
            // Ignore if no consent or not visible
        }
    }
}
