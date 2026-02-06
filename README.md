# Hays Challenge — C# Selenium Automation

Automation project for the [Automation Exercise](https://automationexercise.com) web application using C#, Selenium WebDriver, and NUnit.

## Requirements

- .NET 10.0
- Chrome (for running tests in the browser)
- A file with registered user credentials (see Configuration)

## Running tests

From the solution root:

```bash
dotnet test TestProject
```

## Project structure

- **Browser** — WebDriver factory, base test class
- **Configuration** — settings from `appsettings.json` (browser, timeouts, app URL, payment card, retries)
- **Data** — data loading (e.g. user credentials)
- **Helpers** — utilities (cookies, etc.)
- **Pages** — page objects and components
- **Services** — services (cart, products via API/site)
- **Tests** — NUnit tests (e.g. login → search → add to cart → checkout flow)

## Configuration

Main settings are in `TestProject/appsettings.json`:

- **BrowserSettings** — browser (Chrome), headless, timeouts, window size
- **WebApp** — application base URL
- **PaymentCard** — test card details for payment
- **RegisteredUserFile** — path to the registered user file (e.g. from the `hays-challenge-playwright` project, file `data/registered-user.json` with `email`, `password`, `firstName`, `lastName`, etc.)
- **Retry** — retry count and delay on test failure

Before the first run, either create this file with a user registered on automationexercise.com, or adjust the path in **RegisteredUserFile** to match your repository layout.

## Test scenario

The test `Login_Search_AddToCart_Checkout_PlaceOrder_Success` performs:

1. Login with email and password from the user file
2. Assert successful login (user name displayed)
3. Navigate to Products via the menu
4. Search for a product (name chosen at random from a list obtained via API)
5. Add the first matching product to the cart and open the cart via the modal
6. Assert cart contents and proceed to checkout
7. Fill the form and place the order with the test card
8. Assert the order success message

After the test, the cart is cleared via API (TearDown).
