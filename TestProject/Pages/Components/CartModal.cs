using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationProject.Pages.Components;

/// <summary>
/// Модальное окно корзины (id="cartModal"), появляется после добавления товара в корзину.
/// </summary>
public class CartModal
{
    private static readonly By ModalRoot = By.Id("cartModal");
    private const string AddedToCartMessage = "Your product has been added to cart";
    private static readonly By ViewCartLink = By.CssSelector("a[href*='view_cart']");

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public CartModal(IWebDriver driver, int timeoutSeconds = 15)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
    }

    /// <summary>
    /// Проверяет, что модальное окно видимо и содержит текст "Your product has been added to cart".
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
    /// Нажимает "View Cart" в модальном окне.
    /// </summary>
    public void ClickViewCart()
    {
        var viewCart = _wait.Until(ExpectedConditions.ElementToBeClickable(ViewCartLink));
        viewCart.Click();
    }
}
