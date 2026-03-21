using Microsoft.Playwright;

namespace AutomationLearning.Core.Pages;

/// <summary>
/// Page Object for the SauceDemo checkout complete page.
/// URL: https://www.saucedemo.com/checkout-complete.html
/// Covers both step-two (review) and the final confirmation.
/// </summary>
public class CheckoutCompletePage(IPage page) : BasePage(page)
{
    private ILocator FinishButton => Page.Locator("[data-test='finish']");
    private ILocator SummaryTotal => Page.Locator(".summary_total_label");
    private ILocator SummaryItems => Page.Locator(".cart_item");
    private ILocator ConfirmationHeader => Page.Locator("[data-test='complete-header']");
    private ILocator BackHomeButton => Page.Locator("[data-test='back-to-products']");

    public async Task<bool> IsReviewLoaded() =>
        await FinishButton.IsVisibleAsync();

    public async Task<string> GetOrderTotal() =>
        await SummaryTotal.TextContentAsync() ?? string.Empty;

    public async Task<int> GetReviewItemCount() =>
        await SummaryItems.CountAsync();

    public async Task FinishCheckout() =>
        await FinishButton.ClickAsync();

    public async Task<bool> IsOrderConfirmed() =>
        await ConfirmationHeader.IsVisibleAsync();

    public async Task<string> GetConfirmationText() =>
        await ConfirmationHeader.TextContentAsync() ?? string.Empty;

    public async Task BackToProducts() =>
        await BackHomeButton.ClickAsync();
}
