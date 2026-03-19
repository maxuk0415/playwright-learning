using Microsoft.Playwright;

namespace AutomationLearning.Tests.Pages;

/// <summary>
/// Page Object for the SauceDemo login page.
/// URL: https://www.saucedemo.com
/// </summary>
public class LoginPage(IPage page) : BasePage(page)
{
    // Selectors — using data-test attributes (most stable selector strategy)
    private ILocator UsernameInput => Page.Locator("[data-test='username']");
    private ILocator PasswordInput => Page.Locator("[data-test='password']");
    private ILocator LoginButton => Page.Locator("[data-test='login-button']");
    private ILocator ErrorMessage => Page.Locator("[data-test='error']");

    public async Task GoTo(string baseUrl) =>
        await Page.GotoAsync(baseUrl);

    public async Task Login(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    public async Task<string> GetErrorMessage() =>
        await ErrorMessage.TextContentAsync() ?? string.Empty;

    public async Task<bool> IsErrorVisible() =>
        await ErrorMessage.IsVisibleAsync();
}
