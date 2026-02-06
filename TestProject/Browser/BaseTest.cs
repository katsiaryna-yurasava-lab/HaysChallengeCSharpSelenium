using Allure.Net.Commons;
using AutomationProject.Data;
using AutomationProject.Logging;
using AutomationProject.Models;
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

    private void AttachArtifactsOnFailure()
    {
        var result = TestContext.CurrentContext.Result.Outcome;
        if (result.Status != ResultState.Failed.Status)
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
            log.AppendLine($"Status: {result.Status}");
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
