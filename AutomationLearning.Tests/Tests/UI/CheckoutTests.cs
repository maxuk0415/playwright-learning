using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationLearning.Tests.Helpers;
using AutomationLearning.Tests.Pages;

namespace AutomationLearning.Tests.Tests.UI;

[TestFixture]
[AllureNUnit]
[AllureSuite("UI Tests")]
[AllureFeature("Checkout")]
[Category("UI")]
[Category("Checkout")]
public class CheckoutTests : TestBase
{
    private LoginPage _loginPage = null!;
    private InventoryPage _inventoryPage = null!;
    private CartPage _cartPage = null!;
    private CheckoutInfoPage _checkoutInfoPage = null!;
    private CheckoutCompletePage _checkoutCompletePage = null!;

    [SetUp]
    public async Task SetUpAndLogin()
    {
        _loginPage = new LoginPage(Page);
        _inventoryPage = new InventoryPage(Page);
        _cartPage = new CartPage(Page);
        _checkoutInfoPage = new CheckoutInfoPage(Page);
        _checkoutCompletePage = new CheckoutCompletePage(Page);

        await _loginPage.GoTo(Settings.BaseUrl);
        await _loginPage.Login(Settings.ValidUsername, Settings.ValidPassword);
    }

    [TestCaseSource(typeof(CheckoutTestData), nameof(CheckoutTestData.ValidCustomers))]
    [AllureStory("Happy Path")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
    [Description("Complete checkout flow should succeed for any valid customer profile")]
    public async Task Checkout_HappyPath_ShouldShowOrderConfirmation(
        string firstName, string lastName, string postalCode)
    {
        await _inventoryPage.AddProductToCart("Sauce Labs Backpack");
        await _inventoryPage.GoToCart();

        Assert.That(await _cartPage.GetCartItemCount(), Is.EqualTo(1),
            "Cart should contain 1 item before checkout");

        await _cartPage.ProceedToCheckout();
        Assert.That(await _checkoutInfoPage.IsLoaded(), Is.True,
            "Checkout info page should be visible");

        await _checkoutInfoPage.FillInfo(firstName, lastName, postalCode);
        await _checkoutInfoPage.Continue();

        Assert.That(await _checkoutCompletePage.IsReviewLoaded(), Is.True,
            "Order review page should be visible");

        await _checkoutCompletePage.FinishCheckout();

        Assert.That(await _checkoutCompletePage.IsOrderConfirmed(), Is.True,
            "Order confirmation should be visible");
        Assert.That(await _checkoutCompletePage.GetConfirmationText(),
            Does.Contain("Thank you").IgnoreCase,
            "Confirmation message should say thank you");
    }

    [Test]
    [AllureStory("Multi-item Checkout")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("Checkout with multiple items should show correct item count in review")]
    public async Task Checkout_WithMultipleItems_ShouldShowAllInReview()
    {
        await _inventoryPage.AddProductToCart("Sauce Labs Backpack");
        await _inventoryPage.AddProductToCart("Sauce Labs Bike Light");
        await _inventoryPage.GoToCart();

        Assert.That(await _cartPage.GetCartItemCount(), Is.EqualTo(2));

        await _cartPage.ProceedToCheckout();
        await _checkoutInfoPage.FillInfo("Max", "Chen", "12345");
        await _checkoutInfoPage.Continue();

        Assert.That(await _checkoutCompletePage.GetReviewItemCount(), Is.EqualTo(2),
            "Review should show all 2 items");
    }

    [TestCaseSource(typeof(CheckoutTestData), nameof(CheckoutTestData.InvalidCustomerInputs))]
    [AllureStory("Form Validation")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("Missing required fields should each show a specific validation error")]
    public async Task Checkout_WithMissingField_ShouldShowValidationError(
        string firstName, string lastName, string postalCode, string expectedErrorFragment)
    {
        await _inventoryPage.AddProductToCart("Sauce Labs Backpack");
        await _inventoryPage.GoToCart();
        await _cartPage.ProceedToCheckout();

        await _checkoutInfoPage.FillInfo(firstName, lastName, postalCode);
        await _checkoutInfoPage.Continue();

        Assert.That(await _checkoutInfoPage.IsErrorVisible(), Is.True,
            "Validation error should appear");
        Assert.That(await _checkoutInfoPage.GetErrorMessage(),
            Does.Contain(expectedErrorFragment).IgnoreCase,
            $"Error should mention '{expectedErrorFragment}'");
    }

    [Test]
    [AllureStory("Navigation")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.minor)]
    [Description("Cancelling checkout info should return to cart")]
    public async Task Checkout_CancelOnInfoPage_ShouldReturnToCart()
    {
        await _inventoryPage.AddProductToCart("Sauce Labs Backpack");
        await _inventoryPage.GoToCart();
        await _cartPage.ProceedToCheckout();

        await _checkoutInfoPage.Cancel();

        Assert.That(Page.Url, Does.Contain("cart.html"),
            "Cancelling checkout should return to cart page");
    }

    [Test]
    [AllureStory("Navigation")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.minor)]
    [Description("Cart items should be preserved when navigating back from checkout")]
    public async Task Cart_WhenNavigatingBackFromCheckout_ShouldPreserveItems()
    {
        await _inventoryPage.AddProductToCart("Sauce Labs Backpack");
        await _inventoryPage.GoToCart();

        var itemsBefore = await _cartPage.GetCartItemNames();

        await _cartPage.ProceedToCheckout();
        await _checkoutInfoPage.Cancel();

        var itemsAfter = await _cartPage.GetCartItemNames();

        Assert.That(itemsAfter, Is.EqualTo(itemsBefore),
            "Cart items should be the same after cancelling checkout");
    }
}
