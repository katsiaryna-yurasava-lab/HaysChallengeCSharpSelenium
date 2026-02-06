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
    private static readonly By ViewCartLink = By.CssSelector("a[href*='view_cart']");

    private readonly RetryPolicy _retryPolicy = Policy
        .Handle<StaleElementReferenceException>()
        .Or<ElementClickInterceptedException>()
        .Or<WebDriverException>()
        .WaitAndRetry(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(500)
        );

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public CartModal(IWebDriver driver, int timeoutSeconds = 15)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
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
    /// Clicks "View Cart" in the modal. Retries on failure (e.g. stale element, not clickable).
    /// </summary>
    public void ClickViewCart()
    {
        _retryPolicy.Execute(() =>
        {
            var viewCart = _wait.Until(
                ExpectedConditions.ElementToBeClickable(ViewCartLink));
            viewCart.Click();
        });
    }
}
