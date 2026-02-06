using AutomationProject.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationProject.Pages.Components;

/// <summary>
/// Top menu: ul.shop-menu.pull-right (Home, Products, Cart, Signup / Login, etc.).
/// </summary>
public class ShopMenu
{
    private static readonly By MenuRoot = By.CssSelector("div.shop-menu");
    private static readonly By ProductsLink = By.CssSelector("a[href*='/products']");

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public ShopMenu(IWebDriver driver, int? timeoutSeconds = null)
    {
        _driver = driver;
        var seconds = timeoutSeconds ?? TestConfig.Browser.ExplicitWaitSeconds;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
    }

    /// <summary>
    /// Clicks the "Products" menu item to navigate to /products page.
    /// </summary>
    public void ClickProducts()
    {
        var menu = _wait.Until(ExpectedConditions.ElementIsVisible(MenuRoot));
        var productsLink = menu.FindElement(ProductsLink);
        _wait.Until(ExpectedConditions.ElementToBeClickable(productsLink)).Click();
    }
}
