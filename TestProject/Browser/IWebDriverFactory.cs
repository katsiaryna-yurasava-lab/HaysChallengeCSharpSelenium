using OpenQA.Selenium;

namespace AutomationProject.Browser;

/// <summary>
/// Фабрика создания экземпляра IWebDriver. Тип браузера и опции задаются конфигурацией.
/// </summary>
public interface IWebDriverFactory
{
    /// <summary>
    /// Создаёт и настраивает драйвер согласно конфигурации (Browser, Headless, таймауты и т.д.).
    /// </summary>
    IWebDriver Create();
}
