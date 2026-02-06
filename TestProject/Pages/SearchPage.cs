using OpenQA.Selenium;

namespace AutomationProject.Pages;

public class SearchPage : BasePage
{
    private static readonly By SearchInput = By.CssSelector("input[type='text'][name='search']");
    private static readonly By SearchButton = By.CssSelector("button#submit_search");

    public SearchPage(IWebDriver driver) : base(driver) { }

    public void Search(string searchTerm)
    {
        WaitVisible(SearchInput).Clear();
        WaitVisible(SearchInput).SendKeys(searchTerm);
        WaitVisible(SearchButton).Click();
    }

    /// <summary>
    /// Clicks "Add to cart" on the first product in search results (or product list).
    /// </summary>
    public void AddFirstProductToCart()
    {
        var addToCart = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(
            By.CssSelector("a.add-to-cart")));
        addToCart.Click();
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
