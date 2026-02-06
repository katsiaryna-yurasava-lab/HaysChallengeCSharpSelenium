using OpenQA.Selenium;

namespace AutomationProject.Pages;

public class LoginPage : BasePage
{
    private static readonly By LoginEmail = By.CssSelector("input[data-qa='login-email']");
    private static readonly By LoginPassword = By.CssSelector("input[data-qa='login-password']");
    private static readonly By LoginButton = By.CssSelector("button[data-qa='login-button']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Open()
    {
        Driver.Navigate().GoToUrl(BaseUrl + "/login");
        AcceptConsentIfPresent();
    }

    public void Login(string email, string password)
    {
        WaitVisible(LoginEmail).Clear();
        WaitVisible(LoginEmail).SendKeys(email);
        WaitVisible(LoginPassword).Clear();
        WaitVisible(LoginPassword).SendKeys(password);
        WaitVisible(LoginButton).Click();
    }
}
