using OpenQA.Selenium;
using AutomationProject.Models;

namespace AutomationProject.Pages;

public class CheckoutPage : BasePage
{
    private static readonly By PlaceOrderButton = By.CssSelector("a[href*='payment'], button.stripe-button-el, #submit");
    private static readonly By NameOnCard = By.CssSelector("input[data-qa='name-on-card']");
    private static readonly By CardNumber = By.CssSelector("input[data-qa='card-number']");
    private static readonly By Cvc = By.CssSelector("input[data-qa='cvc']");
    private static readonly By ExpiryMonth = By.CssSelector("input[data-qa='expiry-month']");
    private static readonly By ExpiryYear = By.CssSelector("input[data-qa='expiry-year']");
    private static readonly By PayButton = By.CssSelector("button[data-qa='pay-button'], #submit, input[value='Pay and Confirm Order']");

    public CheckoutPage(IWebDriver driver) : base(driver) { }

    public void PlaceOrder(RegisteredUserData user)
    {
        ScrollToPlaceOrderAndClick();
        FillPaymentAndSubmit(user);
    }

    private void ScrollToPlaceOrderAndClick()
    {
        var placeOrder = WaitVisible(PlaceOrderButton);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", placeOrder);
        Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(PlaceOrderButton)).Click();
    }

    private void FillPaymentAndSubmit(RegisteredUserData user)
    {
        Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(NameOnCard));

        WaitVisible(NameOnCard).SendKeys(user.Name);
        WaitVisible(CardNumber).SendKeys("4111111111111111");
        WaitVisible(Cvc).SendKeys("123");
        WaitVisible(ExpiryMonth).SendKeys("12");
        WaitVisible(ExpiryYear).SendKeys("2030");

        var pay = Driver.FindElements(PayButton);
        if (pay.Count > 0)
            pay[0].Click();
        else
            Driver.FindElement(By.CssSelector("input[value='Pay and Confirm Order'], button#submit")).Click();
    }

    /// <summary>
    /// Returns true if order placed confirmation (e.g. "Order Placed!" or "Congratulations! Your order has been confirmed") is visible.
    /// </summary>
    public bool IsOrderPlacedSuccess()
    {
        try
        {
            var success = By.XPath("//*[contains(text(), 'Order Placed') or contains(text(), 'order has been confirmed') or contains(text(), 'Congratulations')]");
            return IsVisible(success);
        }
        catch
        {
            return false;
        }
    }
}
