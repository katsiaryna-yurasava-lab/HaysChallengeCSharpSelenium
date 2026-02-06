using AutomationProject.Browser;
using AutomationProject.Data;
using AutomationProject.Models;
using AutomationProject.Pages;
using AutomationProject.Pages.Components;
using OpenQA.Selenium;

namespace AutomationProject;

public class CheckoutFlowTests
{
    private IWebDriver _driver = null!;
    private RegisteredUserData _user = null!;
    private IReadOnlyList<string> _productNames = null!;
    private string _selectedProductName = null!;

    [SetUp]
    public void Setup()
    {
        var factory = new WebDriverFactory();
        _driver = factory.Create();
        _user = CredentialsReader.Load();

        // GET /products and collect all product names from HTML (div.single-products > div.productinfo > p)
        _productNames = ProductsApi.GetProductNamesFromProductsPage();
        // Pick a random product for the test
        _selectedProductName = _productNames.Count > 0
            ? _productNames[Random.Shared.Next(_productNames.Count)]
            : "top";
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Dispose();
    }

    [Test]
    public void Login_Search_AddToCart_Checkout_PlaceOrder_Success()
    {
        var loginPage = new LoginPage(_driver);
        var homePage = new HomePage(_driver);
        var searchPage = new SearchPage(_driver);
        var cartPage = new CartPage(_driver);
        var checkoutPage = new CheckoutPage(_driver);

        // 1. Login using credentials from Playwright
        loginPage.Open();
        loginPage.Login(_user.Email, _user.Password);

        // 2. Validate login success
        Assert.That(homePage.IsLoggedInAs(_user.FirstName, _user.LastName), Is.True,
            "Login failed: 'Logged in as' text with user name should be visible.");

        // 3. Navigate to Products page via top menu
        var shopMenu = new ShopMenu(_driver);
        shopMenu.ClickProducts();

        // 4. Search for a product
        searchPage.Search(_selectedProductName);

        // 5. Add product(s) to the cart
        searchPage.AddFirstProductToCart();
        var cartModal = new CartModal(_driver);
        Assert.That(cartModal.IsAddedToCartMessageVisible(), Is.True,
            "After adding to cart, modal #cartModal should appear with message 'Your product has been added to cart'.");
        cartModal.ClickViewCart();

        // 6. Validate product added to cart and proceed to checkout
        Assert.That(cartPage.HasItems(), Is.True, "Cart should contain at least one item.");
        cartPage.ProceedToCheckoutClick();

        // 7. Place the order successfully
        checkoutPage.PlaceOrder(_user);

        // 8. Validate order placed confirmation
        Assert.That(checkoutPage.IsOrderPlacedSuccess(), Is.True,
            "Order placed confirmation should be visible.");
    }
}
