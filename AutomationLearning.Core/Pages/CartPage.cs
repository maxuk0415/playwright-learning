using Microsoft.Playwright;

namespace AutomationLearning.Core.Pages;

/// <summary>
/// Page Object for the SauceDemo shopping cart page.
/// URL: https://www.saucedemo.com/cart.html
/// </summary>
public class CartPage(IPage page) : BasePage(page)
{
    private ILocator CartItems => Page.Locator(".cart_item");
    private ILocator CheckoutButton => Page.Locator("[data-test='checkout']");
    private ILocator ContinueShoppingButton => Page.Locator("[data-test='continue-shopping']");
    private ILocator ShoppingCartLink => Page.Locator(".shopping_cart_link");

    public async Task GoTo() =>
        await Page.GotoAsync("/cart.html");

    public async Task NavigateFromInventory() =>
        await ShoppingCartLink.ClickAsync();

    public async Task<int> GetCartItemCount() =>
        await CartItems.CountAsync();

    public async Task<List<string>> GetCartItemNames()
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

    public async Task ProceedToCheckout() =>
        await CheckoutButton.ClickAsync();

    public async Task ContinueShopping() =>
        await ContinueShoppingButton.ClickAsync();
}
