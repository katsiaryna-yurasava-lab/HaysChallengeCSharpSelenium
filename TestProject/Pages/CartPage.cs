using OpenQA.Selenium;

namespace AutomationProject.Pages;

public class CartPage : BasePage
{
    private static readonly By ProceedToCheckout = By.CssSelector("a.check_out");
    private static readonly By CartItemRows = By.CssSelector("tbody tr[id^='product-']");
    private static readonly By CartDescriptionCell = By.CssSelector("td.cart_description");

    public CartPage(IWebDriver driver) : base(driver) { }

    public void Open()
    {
        Driver.Navigate().GoToUrl(BaseUrl + "/view_cart");
        AcceptConsentIfPresent();
    }

    /// <summary>Number of product rows in the cart (tr id="product-XX").</summary>
    public int GetCartItemCount()
    {
        try
        {
            return Driver.FindElements(CartItemRows).Count;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>Product name shown in the first cart row (from td.cart_description).</summary>
    public string? GetFirstCartItemProductName()
    {
        try
        {
            var rows = Driver.FindElements(CartItemRows);
            if (rows.Count == 0) return null;
            var descCell = rows[0].FindElement(CartDescriptionCell);
            return descCell.Text.Trim();
        }
        catch
        {
            return null;
        }
    }

    public bool HasItems()
    {
        return GetCartItemCount() > 0;
    }

    public void ProceedToCheckoutClick()
    {
        WaitVisible(ProceedToCheckout).Click();
    }
}
