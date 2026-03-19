using Microsoft.Playwright;

namespace AutomationLearning.Tests.Pages;

/// <summary>
/// Page Object for the SauceDemo product inventory page.
/// URL: https://www.saucedemo.com/inventory.html
/// </summary>
public class InventoryPage(IPage page) : BasePage(page)
{
    private ILocator ProductItems => Page.Locator(".inventory_item");
    private ILocator PageTitle => Page.Locator(".title");
    private ILocator ShoppingCartBadge => Page.Locator(".shopping_cart_badge");
    private ILocator SortDropdown => Page.Locator("[data-test='product_sort_container']");

    public async Task<bool> IsLoaded() =>
        await PageTitle.IsVisibleAsync();

    public async Task<int> GetProductCount() =>
        await ProductItems.CountAsync();

    public async Task AddProductToCart(string productName)
    {
        var product = Page.Locator(".inventory_item", new PageLocatorOptions
        {
            HasText = productName
        });
        await product.Locator("button").ClickAsync();
    }

    public async Task<int> GetCartCount()
    {
        if (!await ShoppingCartBadge.IsVisibleAsync())
            return 0;

        var text = await ShoppingCartBadge.TextContentAsync();
        return int.TryParse(text, out var count) ? count : 0;
    }

    public async Task GoToCart() =>
        await Page.Locator(".shopping_cart_link").ClickAsync();

    public async Task SortBy(string option) =>
        await SortDropdown.SelectOptionAsync(new SelectOptionValue { Label = option });

    public async Task<List<string>> GetProductNames()
    {
        var names = new List<string>();
        var nameElements = Page.Locator(".inventory_item_name");
        var count = await nameElements.CountAsync();

        for (int i = 0; i < count; i++)
        {
            var name = await nameElements.Nth(i).TextContentAsync();
            if (name != null) names.Add(name);
        }
        return names;
    }
}
