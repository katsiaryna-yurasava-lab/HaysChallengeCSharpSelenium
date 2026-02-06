using AutomationProject.Browser;
using AutomationProject.Helpers;
using AutomationProject.Pages;
using AutomationProject.Pages.Components;
using AutomationProject.Services;

namespace AutomationProject;

public class CheckoutFlowTests : BaseTest
{
    private IReadOnlyList<string> _productNames = null!;
    private string _selectedProductName = null!;

    [SetUp]
    public void CheckoutSetUp()
    {
        _productNames = ProductService.GetProductNamesAsync().GetAwaiter().GetResult();
        if (!_productNames.Any())
            throw new InvalidOperationException("No products returned from API.");
        _selectedProductName = _productNames[Random.Shared.Next(_productNames.Count)];
    }

    protected override async Task OnTearDownAsync()
    {
        var cookieContainer = WebDriverCookieHelper.FromDriver(Driver, Logger);
        await CartService.ClearCartAsync(cookieContainer, Logger);
    }

    [Test]
    public void Login_Search_AddToCart_Checkout_PlaceOrder_Success()
    {
        var loginPage = new LoginPage(Driver);
        var homePage = new HomePage(Driver);
        var productsPage = new ProductsPage(Driver);
        var cartPage = new CartPage(Driver);
        var checkoutPage = new CheckoutPage(Driver);

        // 1. Login using credentials from Playwright
        loginPage.Open();
        loginPage.Login(User.Email, User.Password);

        // 2. Validate login success
        Assert.That(homePage.IsLoggedInAs(User.FirstName, User.LastName), Is.True,
            "Login failed: 'Logged in as' text with user name should be visible.");

        // 3. Navigate to Products page via top menu
        var shopMenu = new ShopMenu(Driver);
        shopMenu.ClickProducts();

        // 4. Search for a product
        productsPage.Search(_selectedProductName);

        // 5. Add product(s) to the cart
        productsPage.AddFirstProductToCart();
        var cartModal = new CartModal(Driver);
        Assert.That(cartModal.IsAddedToCartMessageVisible(), Is.True,
            "After adding to cart, modal #cartModal should appear with message 'Your product has been added to cart'.");
        cartModal.ClickViewCart();

        // 6. Validate exactly one product in cart and its data matches what was added
        Assert.That(cartPage.GetCartItemCount(), Is.EqualTo(1),
            "Cart should contain exactly one item.");
        Assert.That(cartPage.GetFirstCartItemProductName(), Does.Contain(_selectedProductName),
            "Cart item description should match the added product: " + _selectedProductName);
        cartPage.ProceedToCheckoutClick();

        // 7. Place the order successfully
        checkoutPage.PlaceOrder(User);

        // 8. Validate order placed confirmation
        Assert.That(checkoutPage.IsOrderPlacedElementVisible(), Is.True,
            "Element [data-qa='order-placed'] with text 'Order Placed!' should be visible.");
        Assert.That(checkoutPage.IsOrderConfirmedTextVisible(), Is.True,
            "Page should contain text 'Congratulations! Your order has been confirmed!'.");
    }
}
