using AutomationProject.Data;
using AutomationProject.Logging;
using AutomationProject.Models;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace AutomationProject.Browser;

public abstract class BaseTest
{
    protected IWebDriver Driver = null!;
    protected ILogger Logger = null!;
    protected RegisteredUserData User = null!;

    [SetUp]
    public void SetUp()
    {
        Logger = TestLoggerFactory.CreateLogger(GetType());
        Driver = new WebDriverFactory().Create();
        User = CredentialsReader.Load();
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        await OnTearDownAsync();
        Driver?.Dispose();
    }

    /// <summary>Override to add cleanup (e.g. clear cart) before driver is disposed.</summary>
    protected virtual Task OnTearDownAsync() => Task.CompletedTask;
}
