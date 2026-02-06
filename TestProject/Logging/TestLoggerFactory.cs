using Microsoft.Extensions.Logging;

namespace AutomationProject.Logging;

/// <summary>
/// Shared LoggerFactory for tests (console output). Use CreateLogger to get an ILogger.
/// </summary>
public static class TestLoggerFactory
{
    public static readonly ILoggerFactory Factory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Debug);
    });

    public static ILogger CreateLogger<T>() => Factory.CreateLogger<T>();

    public static ILogger CreateLogger(string categoryName) => Factory.CreateLogger(categoryName);
}
