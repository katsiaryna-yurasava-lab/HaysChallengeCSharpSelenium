using Allure.Net.Commons;
using AutomationProject.Data;
using AutomationProject.Logging;
using AutomationProject.Models;
using AutomationProject.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace AutomationProject.Browser;

[Allure.NUnit.AllureNUnit]
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
        AttachArtifactsOnFailure();
        await OnTearDownAsync();
        Driver?.Dispose();
    }

    /// <summary>Override to add cleanup (e.g. clear cart) before driver is disposed.</summary>
    protected virtual Task OnTearDownAsync() => Task.CompletedTask;

    /// <summary>Page Factory: create a page instance, e.g. var loginPage = Page&lt;LoginPage&gt;();</summary>
    protected TPage Page<TPage>() where TPage : BasePage =>
        (TPage)Activator.CreateInstance(typeof(TPage), Driver)!;

    /// <summary>Component/Modal Factory: create a component with (IWebDriver, optional timeout), e.g. var cartModal = Component&lt;CartModal&gt;();</summary>
    protected T Component<T>() where T : class =>
        (T)Activator.CreateInstance(typeof(T), Driver, null)!;

    private void AttachArtifactsOnFailure()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome;
        if (outcome.Status != TestStatus.Failed)
            return;

        try
        {
            if (Driver is ITakesScreenshot takesScreenshot)
            {
                var screenshot = takesScreenshot.GetScreenshot();
                var bytes = screenshot.AsByteArray;
                if (bytes is { Length: > 0 })
                {
                    var name = $"screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    AllureApi.AddAttachment(name, "image/png", bytes, "png");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to capture screenshot for Allure");
        }

        try
        {
            var log = new System.Text.StringBuilder();
            log.AppendLine($"Test: {TestContext.CurrentContext.Test.Name}");
            log.AppendLine($"Status: {outcome.Status}");
            log.AppendLine($"Message: {TestContext.CurrentContext.Result.Message}");
            if (!string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace))
                log.AppendLine($"StackTrace:\n{TestContext.CurrentContext.Result.StackTrace}");
            var logBytes = System.Text.Encoding.UTF8.GetBytes(log.ToString());
            AllureApi.AddAttachment("failure_log.txt", "text/plain", logBytes, "txt");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to attach failure log to Allure");
        }
    }
}
