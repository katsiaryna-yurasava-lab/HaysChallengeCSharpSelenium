namespace AutomationProject.Configuration;

/// <summary>
/// Настройки браузера для тестов. Загружаются из appsettings.json и переменных окружения.
/// </summary>
public class BrowserSettings
{
    /// <summary>Тип браузера: Chrome, Firefox, Edge.</summary>
    public string Browser { get; set; } = "Chrome";

    /// <summary>Запуск в headless-режиме.</summary>
    public bool Headless { get; set; }

    /// <summary>Таймаут неявного ожидания (секунды).</summary>
    public int ImplicitWaitSeconds { get; set; } = 5;

    /// <summary>Таймаут загрузки страницы (секунды).</summary>
    public int PageLoadTimeoutSeconds { get; set; } = 30;

    /// <summary>Разворачивать окно на весь экран.</summary>
    public bool WindowMaximize { get; set; } = true;
}
