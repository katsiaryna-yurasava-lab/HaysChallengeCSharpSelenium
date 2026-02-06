namespace AutomationProject.Configuration;

/// <summary>
/// Настройки пути к файлу с данными зарегистрированного пользователя (созданного Playwright).
/// </summary>
public class RegisteredUserFileSettings
{
    /// <summary>Имя папки проекта с Playwright (на уровне выше текущего проекта).</summary>
    public string RelativePathFromSolution { get; set; } = "hays-challenge-playwright";

    /// <summary>Папка с данными внутри проекта Playwright.</summary>
    public string DataFolder { get; set; } = "data";

    /// <summary>Имя файла с данными пользователя.</summary>
    public string FileName { get; set; } = "registered-user.json";
}
