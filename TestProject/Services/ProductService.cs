using HtmlAgilityPack;

namespace AutomationProject.Services;

/// <summary>
/// Product-related operations via HTTP only. Uses HttpClient for GET /products and parses product names from HTML.
/// </summary>
public static class ProductService
{
    private const string UserAgent = "Mozilla/5.0 (compatible; Selenium-Test)";

    /// <summary>
    /// GET baseUrl/products, parses HTML for .single-products .productinfo p, returns product names.
    /// </summary>
    public static async Task<IReadOnlyList<string>> GetProductNamesAsync(string baseUrl, CancellationToken cancellationToken = default)
    {
        using var http = CreateHttpClient();
        var html = await GetProductsPageHtmlAsync(http, baseUrl, cancellationToken);
        return ParseProductNamesFromHtml(html);
    }

    /// <summary>
    /// GET baseUrl/products — returns products page HTML.
    /// </summary>
    public static async Task<string> GetProductsPageHtmlAsync(
        HttpClient http,
        string baseUrl,
        CancellationToken cancellationToken = default)
    {
        var url = baseUrl.TrimEnd('/') + "/products";
        return await http.GetStringAsync(url, cancellationToken);
    }

    /// <summary>
    /// Parses products page HTML and returns all product names from .single-products .productinfo p.
    /// </summary>
    public static IReadOnlyList<string> ParseProductNamesFromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var productNames = new List<string>();
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

    private static HttpClient CreateHttpClient()
    {
        var http = new HttpClient();
        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        return http;
    }
}
