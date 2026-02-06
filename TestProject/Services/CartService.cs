using System.Net;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace AutomationProject.Services;

/// <summary>
/// Cart-related operations via HTTP only. Uses HttpClient with provided CookieContainer for view_cart and delete_cart.
/// </summary>
public static class CartService
{
    private const string UserAgent = "Mozilla/5.0 (compatible; Selenium-Test)";

    /// <summary>
    /// Clears the cart: fetches cart page, finds all product row ids, then deletes each item.
    /// </summary>
    public static async Task ClearCartAsync(
        string baseUrl,
        CookieContainer cookieContainer,
        ILogger? logger = null,
        CancellationToken cancellationToken = default)
    {
        var baseUrlNorm = baseUrl.TrimEnd('/');
        using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var http = CreateHttpClient(handler);

        var html = await GetCartPageHtmlAsync(http, baseUrlNorm, logger, cancellationToken);
        if (string.IsNullOrEmpty(html)) return;

        var productIds = FindProductIdsInCartHtml(html);
        foreach (var id in productIds)
        {
            try
            {
                await DeleteCartItemAsync(http, baseUrlNorm, id, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                logger?.LogWarning(ex, "Failed to delete cart item {Id}", id);
            }
        }
    }

    /// <summary>
    /// GET view_cart — returns cart page HTML or null on failure.
    /// </summary>
    public static async Task<string?> GetCartPageHtmlAsync(
        HttpClient http,
        string baseUrl,
        ILogger? logger = null,
        CancellationToken cancellationToken = default)
    {
        var url = baseUrl.TrimEnd('/') + "/view_cart";
        try
        {
            return await http.GetStringAsync(url, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger?.LogWarning(ex, "Failed to get ViewCart HTML");
            return null;
        }
    }

    /// <summary>
    /// Parses cart page HTML and returns all product row ids (e.g. "21", "22") from tr id="product-XX".
    /// </summary>
    public static IReadOnlyList<string> FindProductIdsInCartHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var ids = new List<string>();
        var rows = doc.DocumentNode.SelectNodes("//tr[starts-with(@id, 'product-')]");
        if (rows == null) return ids;

        foreach (var row in rows)
        {
            var idAttr = row.GetAttributeValue("id", null);
            if (string.IsNullOrEmpty(idAttr) || !idAttr.StartsWith("product-", StringComparison.OrdinalIgnoreCase))
                continue;
            var id = idAttr["product-".Length..].Trim();
            if (!string.IsNullOrEmpty(id))
                ids.Add(id);
        }

        return ids;
    }

    /// <summary>
    /// GET delete_cart/{productId} — removes one item from the cart.
    /// </summary>
    public static async Task DeleteCartItemAsync(
        HttpClient http,
        string baseUrl,
        string productId,
        CancellationToken cancellationToken = default)
    {
        var url = baseUrl.TrimEnd('/') + "/delete_cart/" + productId;
        await http.GetAsync(url, cancellationToken);
    }

    private static HttpClient CreateHttpClient(HttpClientHandler handler)
    {
        var http = new HttpClient(handler);
        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        return http;
    }
}
