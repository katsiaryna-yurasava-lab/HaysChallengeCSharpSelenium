using System.Text.Json;
using AutomationProject.Configuration;
using AutomationProject.Models;

namespace AutomationProject.Data;

/// <summary>
/// Reads registered user credentials from the shared file created by Playwright (Task 1).
/// Path is set in configuration: RegisteredUserFile section in appsettings.json.
/// </summary>
public static class CredentialsReader
{
    /// <summary>
    /// Resolves the path to the registered user file from configuration (RelativePathFromSolution/DataFolder/FileName).
    /// </summary>
    public static string GetRegisteredUserFilePath()
    {
        var settings = TestConfig.RegisteredUserFile;
        var relativePath = settings.RelativePathFromSolution.Trim();
        var dataFolder = settings.DataFolder.Trim();
        var fileName = settings.FileName.Trim();

        var baseDir = AppContext.BaseDirectory;
        var dir = new DirectoryInfo(baseDir);

        while (dir != null)
        {
            var siblingPath = Path.Combine(dir.Parent?.FullName ?? "", relativePath, dataFolder, fileName);
            if (File.Exists(siblingPath))
                return Path.GetFullPath(siblingPath);

            var childPath = Path.Combine(dir.FullName, relativePath, dataFolder, fileName);
            if (File.Exists(childPath))
                return Path.GetFullPath(childPath);

            dir = dir.Parent;
        }

        var projectDir = Path.GetDirectoryName(typeof(CredentialsReader).Assembly.Location);
        for (int i = 0; i < 6 && !string.IsNullOrEmpty(projectDir); i++)
        {
            projectDir = Path.GetDirectoryName(projectDir);
            var candidate = Path.Combine(projectDir ?? "", "..", relativePath, dataFolder, fileName);
            var full = Path.GetFullPath(candidate);
            if (File.Exists(full))
                return full;
        }

        throw new FileNotFoundException(
            $"Registered user file not found. Check config RegisteredUserFile: {relativePath}/{dataFolder}/{fileName}");
    }

    /// <summary>
    /// Loads and deserializes the registered user data. No hardcoded credentials.
    /// </summary>
    public static RegisteredUserData Load()
    {
        var path = GetRegisteredUserFilePath();
        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<RegisteredUserData>(json)
            ?? throw new InvalidOperationException($"Failed to deserialize {path}");
        if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Password))
            throw new InvalidOperationException("Registered user file must contain email and password.");
        return data;
    }
}
