using NUnit.Framework;

// Assembly-level SetUpFixture (no namespace) so Allure finds allureConfig.json
// when running via "dotnet test" from solution root.
[SetUpFixture]
public static class AllureSetup
{
    private const string AllureConfigEnvVariable = "ALLURE_CONFIG";

    [OneTimeSetUp]
    public static void EnsureAllureConfigPath()
    {
        if (Environment.GetEnvironmentVariable(AllureConfigEnvVariable) != null)
            return;

        var baseDir = AppContext.BaseDirectory;
        var configPath = Path.Combine(baseDir, "allureConfig.json");
        if (File.Exists(configPath))
            Environment.SetEnvironmentVariable(AllureConfigEnvVariable, configPath);
    }
}
