namespace AutomationProject.Configuration;

/// <summary>
/// Settings for the path to the registered user data file (created by Playwright).
/// </summary>
public class RegisteredUserFileSettings
{
    /// <summary>Name of the Playwright project folder (one level above the current project).</summary>
    public string RelativePathFromSolution { get; set; } = "hays-challenge-playwright";

    /// <summary>Data folder inside the Playwright project.</summary>
    public string DataFolder { get; set; } = "data";

    /// <summary>Name of the user data file.</summary>
    public string FileName { get; set; } = "registered-user.json";
}
