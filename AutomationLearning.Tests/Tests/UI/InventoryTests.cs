using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationLearning.Tests.Helpers;
using AutomationLearning.Core.Pages;

namespace AutomationLearning.Tests.Tests.UI;

[TestFixture]
[AllureNUnit]
[AllureSuite("UI Tests")]
[AllureFeature("Product Inventory")]
[Category("UI")]
[Category("Inventory")]
public class InventoryTests : TestBase
{
    private LoginPage _loginPage = null!;
    private InventoryPage _inventoryPage = null!;

    [SetUp]
    public async Task SetUpAndLogin()
    {
        _loginPage = new LoginPage(Page);
        _inventoryPage = new InventoryPage(Page);

        await _loginPage.GoTo(Settings.BaseUrl);
        await _loginPage.Login(Settings.ValidUsername, Settings.ValidPassword);
    }

    [Test]
    [AllureStory("Product Display")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("Inventory page should display 6 products")]
    public async Task Inventory_ShouldDisplay6Products()
    {
        var count = await _inventoryPage.GetProductCount();
        Assert.That(count, Is.EqualTo(6), "There should be exactly 6 products");
    }

    [Test]
    [AllureStory("Add to Cart")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
    [Description("Adding a product to cart should update the cart badge")]
    public async Task AddToCart_ShouldUpdateCartBadge()
    {
        await _inventoryPage.AddProductToCart("Sauce Labs Backpack");

        var cartCount = await _inventoryPage.GetCartCount();
        Assert.That(cartCount, Is.EqualTo(1), "Cart should show 1 item");
    }

    [Test]
    [AllureStory("Product Sorting")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.minor)]
    [Description("Sorting by Name Z-A should reorder products")]
    public async Task Sort_ByNameZtoA_ShouldReverseOrder()
    {
        var beforeSort = await _inventoryPage.GetProductNames();
        await _inventoryPage.SortBy("Name (Z to A)");
        var afterSort = await _inventoryPage.GetProductNames();

        Assert.That(afterSort, Is.Not.EqualTo(beforeSort),
            "Product order should change after sorting");
        Assert.That(afterSort[0], Is.EqualTo(beforeSort[^1]),
            "First item after Z-A sort should be last item before sort");
    }
}
