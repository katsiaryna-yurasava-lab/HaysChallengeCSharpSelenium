using AutomationProject.Configuration;
using AutomationProject.Models;
using OpenQA.Selenium;

namespace AutomationProject.Pages;

public class CheckoutPage : BasePage
{
    private static readonly By PlaceOrderButton = By.CssSelector("a[href*='payment']");
    private static readonly By NameOnCard = By.CssSelector("input[data-qa='name-on-card']");
    private static readonly By CardNumber = By.CssSelector("input[data-qa='card-number']");
    private static readonly By Cvc = By.CssSelector("input[data-qa='cvc']");
    private static readonly By ExpiryMonth = By.CssSelector("input[data-qa='expiry-month']");
    private static readonly By ExpiryYear = By.CssSelector("input[data-qa='expiry-year']");
    private static readonly By PayButton = By.CssSelector("button[data-qa='pay-button']");

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

        var card = TestConfig.PaymentCard;
        WaitVisible(NameOnCard).SendKeys(user.Name);
        WaitVisible(CardNumber).SendKeys(card.CardNumber);
        WaitVisible(Cvc).SendKeys(card.Cvc);
        WaitVisible(ExpiryMonth).SendKeys(card.ExpiryMonth);
        WaitVisible(ExpiryYear).SendKeys(card.ExpiryYear);
        WaitVisible(PayButton).Click();

    }

    private static readonly By OrderPlacedElement = By.CssSelector("[data-qa='order-placed']");
    private const string OrderPlacedText = "Order Placed!";
    private const string OrderConfirmedText = "Congratulations! Your order has been confirmed!";

    /// <summary>
    /// Returns true if element [data-qa="order-placed"] is visible and contains text "Order Placed!".
    /// </summary>
    public bool IsOrderPlacedElementVisible()
    {
        try
        {
            var el = WaitVisible(OrderPlacedElement);
            return el.Text.Trim().Contains(OrderPlacedText, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns true if the page contains the text "Congratulations! Your order has been confirmed!".
    /// </summary>
    public bool IsOrderConfirmedTextVisible()
    {
        try
        {
            var body = Driver.FindElement(By.TagName("body"));
            return body.Text.Contains(OrderConfirmedText, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
