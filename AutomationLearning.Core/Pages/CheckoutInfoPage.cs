using Microsoft.Playwright;

namespace AutomationLearning.Core.Pages;

/// <summary>
/// Page Object for the SauceDemo checkout step one (customer info).
/// URL: https://www.saucedemo.com/checkout-step-one.html
/// </summary>
public class CheckoutInfoPage(IPage page) : BasePage(page)
{
    private ILocator FirstNameInput => Page.Locator("[data-test='firstName']");
    private ILocator LastNameInput => Page.Locator("[data-test='lastName']");
    private ILocator PostalCodeInput => Page.Locator("[data-test='postalCode']");
    private ILocator ContinueButton => Page.Locator("[data-test='continue']");
    private ILocator CancelButton => Page.Locator("[data-test='cancel']");
    private ILocator ErrorMessage => Page.Locator("[data-test='error']");

    public async Task<bool> IsLoaded() =>
        await FirstNameInput.IsVisibleAsync();

    public async Task FillInfo(string firstName, string lastName, string postalCode)
    {
        await FirstNameInput.FillAsync(firstName);
        await LastNameInput.FillAsync(lastName);
        await PostalCodeInput.FillAsync(postalCode);
    }

    public async Task Continue() =>
        await ContinueButton.ClickAsync();

    public async Task Cancel() =>
        await CancelButton.ClickAsync();

    public async Task<bool> IsErrorVisible() =>
        await ErrorMessage.IsVisibleAsync();

    public async Task<string> GetErrorMessage() =>
        await ErrorMessage.TextContentAsync() ?? string.Empty;
}
