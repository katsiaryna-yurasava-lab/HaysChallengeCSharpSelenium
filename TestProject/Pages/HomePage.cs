using OpenQA.Selenium;

namespace AutomationProject.Pages;

public class HomePage : BasePage
{
    private static readonly By LoggedInText = By.XPath("//*[contains(text(), 'Logged in as')]");

    public HomePage(IWebDriver driver) : base(driver) { }

    public void Open()
    {
        Driver.Navigate().GoToUrl(BaseUrl);
        AcceptConsentIfPresent();
    }

    /// <summary>
    /// Asserts that the user is logged in and the header shows "Logged in as FirstName LastName".
    /// </summary>
    public bool IsLoggedInAs(string firstName, string lastName)
    {
        var el = WaitVisible(LoggedInText);
        var text = el.Text;
        return text.Contains("Logged in as", StringComparison.OrdinalIgnoreCase)
               && text.Contains(firstName, StringComparison.OrdinalIgnoreCase)
               && text.Contains(lastName, StringComparison.OrdinalIgnoreCase);
    }
}
