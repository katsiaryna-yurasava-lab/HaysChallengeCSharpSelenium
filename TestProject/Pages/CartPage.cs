using OpenQA.Selenium;

namespace AutomationProject.Pages;

public class CartPage : BasePage
{
    private static readonly By ProceedToCheckout = By.CssSelector("a.check_out");

    public CartPage(IWebDriver driver) : base(driver) { }

    public void Open()
    {
        Driver.Navigate().GoToUrl(BaseUrl + "/view_cart");
        AcceptConsentIfPresent();
    }

    public bool HasItems()
    {
        try
        {
            return Driver.FindElements(By.CssSelector("tbody tr.cart_item, #cart_info_table tbody tr")).Count > 0;
        }
        catch
        {
            return false;
        }
    }

    public void ProceedToCheckoutClick()
    {
        WaitVisible(ProceedToCheckout).Click();
    }
}
