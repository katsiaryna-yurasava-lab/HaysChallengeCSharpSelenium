using AutomationProject.Configuration;
using HtmlAgilityPack;

namespace AutomationProject.Data;

/// <summary>
/// Загружает список наименований продуктов со страницы /products через GET-запрос и парсинг HTML.
/// Структура: div.single-products > div.productinfo.text-center > p (текст — название продукта).
/// </summary>
public static class ProductsApi
{
    /// <summary>
    /// Выполняет GET на BaseUrl/products и извлекает все названия продуктов из блоков .single-products .productinfo p.
    /// </summary>
    public static IReadOnlyList<string> GetProductNamesFromProductsPage()
    {
        var baseUrl = TestConfig.WebApp.BaseUrl.TrimEnd('/');
        var url = baseUrl + "/products";

        using var http = new HttpClient();
        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; Selenium-Test)");
        var html = http.GetStringAsync(url).GetAwaiter().GetResult();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // div.single-products > div.productinfo (text-center) > p
        var productNames = new List<string>();
        // div.single-products > div.productinfo.text-center > p
        var productInfoNodes = doc.DocumentNode.SelectNodes(
            "//div[contains(@class,'single-products')]//div[contains(@class,'productinfo')]//p");

        if (productInfoNodes != null)
        {
            foreach (var node in productInfoNodes)
            {
                var name = node.InnerText.Trim();
                if (!string.IsNullOrEmpty(name))
                    productNames.Add(name);
            }
        }

        return productNames;
    }
}
