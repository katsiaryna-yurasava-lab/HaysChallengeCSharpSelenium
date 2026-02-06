using AutomationProject.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Polly;
using Polly.Retry;
using SeleniumExtras.WaitHelpers;

namespace AutomationProject.Pages.Components;

/// <summary>
/// Cart modal (id="cartModal"), shown after adding a product to cart.
/// </summary>
public class CartModal
{
    private static readonly By ModalRoot = By.Id("cartModal");
    private const string AddedToCartMessage = "Your product has been added to cart";
    private readonly RetryPolicy _retryPolicy;
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public CartModal(IWebDriver driver, int? timeoutSeconds = null)
    {
        _driver = driver;
        var seconds = timeoutSeconds ?? TestConfig.Browser.ExplicitWaitSeconds;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

        var retry = TestConfig.Retry;
        _retryPolicy = Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<WebDriverException>()
            .WaitAndRetry(
                retryCount: retry.RetryCount,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(retry.SleepDurationMilliseconds)
            );
    }

    /// <summary>
    /// Checks that the modal is visible and contains the text "Your product has been added to cart".
    /// </summary>
    public bool IsAddedToCartMessageVisible()
    {
        try
        {
            var modal = _wait.Until(ExpectedConditions.ElementIsVisible(ModalRoot));
            return modal.Text.Contains(AddedToCartMessage, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Clicks "View Cart" in the modal. Uses link inside #cartModal and JS click to avoid modal intercepting.
    /// </summary>
    public void ClickViewCart()
    {
        _retryPolicy.Execute(() =>
        {
            var modal = _wait.Until(ExpectedConditions.ElementIsVisible(ModalRoot));
            var viewCart = modal.FindElement(By.CssSelector("a[href*='view_cart']"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", viewCart);
        });
    }
}
