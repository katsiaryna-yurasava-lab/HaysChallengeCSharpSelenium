namespace AutomationProject.Configuration;

/// <summary>
/// Retry policy settings (e.g. for Polly WaitAndRetry). Used by CartModal and other components.
/// </summary>
public class RetrySettings
{
    /// <summary>Number of retry attempts.</summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>Delay between retries (milliseconds).</summary>
    public int SleepDurationMilliseconds { get; set; } = 500;
}
