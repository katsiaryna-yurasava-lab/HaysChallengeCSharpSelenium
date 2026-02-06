using AutomationProject.Configuration;
using OpenQA.Selenium;
using Polly;
using Polly.Retry;
using SeleniumExtras.WaitHelpers;

namespace AutomationProject.Pages;

public class ProductsPage : BasePage
{
    private static readonly By SearchInput = By.CssSelector("input[type='text'][name='search']");
    private static readonly By SearchButton = By.CssSelector("button#submit_search");
    private static readonly By AddToCartButton = By.CssSelector("a.add-to-cart");

    public ProductsPage(IWebDriver driver) : base(driver) { }

    public void Search(string searchTerm)
    {
        WaitVisible(SearchInput).Clear();
        WaitVisible(SearchInput).SendKeys(searchTerm);
        WaitVisible(SearchButton).Click();
    }

    /// <summary>
    /// Clicks "Add to cart" on the first product in search results. Uses retry (config Retry section) on click intercepted / stale.
    /// </summary>
    public void AddFirstProductToCart()
    {
        var retry = TestConfig.Retry;
        var retryPolicy = Policy
            .Handle<StaleElementReferenceException>()
            .Or<ElementClickInterceptedException>()
            .Or<WebDriverException>()
            .WaitAndRetry(
                retryCount: retry.RetryCount,
                sleepDurationProvider: _ => TimeSpan.FromMilliseconds(retry.SleepDurationMilliseconds)
            );

        retryPolicy.Execute(() =>
        {
            var addToCart = Wait.Until(ExpectedConditions.ElementToBeClickable(AddToCartButton));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", addToCart);
            addToCart.Click();
        });
    }

    /// <summary>
    /// Navigate to cart: click "View Cart" in the modal or go by URL if modal is not available.
    /// </summary>
    public void ViewCart()
    {
        try
        {
            var cartModal = new Components.CartModal(Driver);
            cartModal.ClickViewCart();
        }
        catch
        {
            Driver.Navigate().GoToUrl(BaseUrl + "/view_cart");
        }
    }
}
