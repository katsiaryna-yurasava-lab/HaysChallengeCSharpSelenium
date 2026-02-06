# Hays Challenge — C# Selenium Automation

Automation project for the [Automation Exercise](https://automationexercise.com) web application using C#, Selenium WebDriver, and NUnit. Implements an end-to-end checkout flow: login, product search, add to cart, and place order with test card payment.

## Tech stack

- **.NET 10** — target framework  
- **Selenium WebDriver 4** — browser automation (Chrome)  
- **NUnit 4** — test framework  
- **Microsoft.Extensions.Configuration** — JSON and environment configuration  
- **Allure.NUnit** — test reporting  
- **Polly** — retry policies  
- **HtmlAgilityPack** — HTML parsing (e.g. product API responses)

## Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Chrome browser (for running tests)
- A registered user credentials file (see [Configuration](#configuration))

## Getting started

1. Clone the repository and restore dependencies:

   ```bash
   dotnet restore
   ```

2. Configure `TestProject/appsettings.json` and ensure the registered user file path is correct (see [Configuration](#configuration)).

3. Run tests:

   ```bash
   dotnet test TestProject
   ```

## Running tests

From the solution root:

```bash
dotnet test TestProject
```

Run a specific test class:

```bash
dotnet test TestProject --filter "FullyQualifiedName~CheckoutFlowTests"
```

Allure results are written to **`TestProject/allure-results`** (configured in `TestProject/allureConfig.json`). After running tests, generate and open an Allure report:

```bash
cd TestProject
allure generate allure-results -o allure-report --clean
allure open allure-report
```

(Requires [Allure CLI](https://docs.qameta.io/allure/#_installing_a_commandline) and Java.)

If no results appear, ensure `dotnet test` completed successfully and that `TestProject/allure-results` exists. The project sets `ALLURE_CONFIG` in an assembly SetUpFixture so the config next to the test assembly is used when running via `dotnet test`.

## Configuration

Main settings are in **`TestProject/appsettings.json`**:

| Section | Description |
|--------|-------------|
| **BrowserSettings** | Browser (Chrome), headless, implicit/page load/explicit timeouts, window maximize |
| **WebApp** | Application base URL (`https://automationexercise.com`) |
| **PaymentCard** | Test card details for payment (number, CVC, expiry) |
| **RegisteredUserFile** | Path to the registered user JSON file (see below) |
| **Retry** | Retry count and delay (ms) on test failure |

### Registered user file

Tests log in and fill checkout using a JSON file. You can reuse the file from the **hays-challenge-playwright** project:

- **RelativePathFromSolution**: e.g. `"hays-challenge-playwright"` (sibling folder)
- **DataFolder**: e.g. `"data"`
- **FileName**: e.g. `"registered-user.json"`

The file is resolved relative to the solution directory. Expected structure (matches Playwright Task 1 output):

```json
{
  "name": "Full Name",
  "email": "user@example.com",
  "password": "password",
  "title": "Mr",
  "firstName": "First",
  "lastName": "Last",
  "company": "Company",
  "address": "Address line 1",
  "address2": null,
  "country": "Country",
  "state": "State",
  "city": "City",
  "zipcode": "12345",
  "mobileNumber": "1234567890",
  "day": 1,
  "month": 1,
  "year": 1990
}
```

Before the first run, either create this file with a user registered on [automationexercise.com](https://automationexercise.com), or adjust **RegisteredUserFile** in `appsettings.json` to match your layout.

## Project structure

| Folder | Purpose |
|--------|--------|
| **Browser** | WebDriver factory, base test class (driver lifecycle, page/component factories, screenshot on failure) |
| **Configuration** | Settings classes bound from `appsettings.json` (browser, web app, payment card, retries, user file) |
| **Data** | Data loading (e.g. `CredentialsReader` for registered user JSON) |
| **Helpers** | Utilities (e.g. `WebDriverCookieHelper` for API calls with session cookies) |
| **Logging** | `TestLoggerFactory` for test logging |
| **Models** | DTOs (e.g. `RegisteredUserData`) |
| **Pages** | Page Object Model: `BasePage`, `LoginPage`, `HomePage`, `ProductsPage`, `CartPage`, `CheckoutPage`, and components (`CartModal`, `ShopMenu`) |
| **Services** | `ProductService` (product names via API), `CartService` (clear cart via API) |
| **Tests** | NUnit test classes (e.g. `CheckoutFlowTests`) |

## Test scenario

The test **`Login_Search_AddToCart_Checkout_PlaceOrder_Success`** performs:

1. **Login** with email and password from the registered user file  
2. **Assert** successful login (user name displayed in header)  
3. **Navigate** to Products via the top menu  
4. **Search** for a product (name chosen at random from a list obtained via the site API)  
5. **Add** the first matching product to the cart and open the cart via the modal  
6. **Assert** cart contents (one item, name matches) and proceed to checkout  
7. **Fill** checkout form and place the order using the test payment card  
8. **Assert** order success message (“Order Placed!”, “Congratulations! Your order has been confirmed!”)  

**TearDown:** The cart is cleared via API after the test so the account is left in a clean state.

## Other behaviour

- **Retries:** Failed tests are retried according to `Retry` settings in `appsettings.json`.  
- **Screenshots:** On failure, a screenshot is attached to the test result (and to Allure when used).  
- **Allure:** Tests are annotated for Allure; use `allureConfig.json` and the Allure CLI to generate reports.
