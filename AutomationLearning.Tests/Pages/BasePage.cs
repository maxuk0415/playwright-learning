using Microsoft.Playwright;

namespace AutomationLearning.Tests.Pages;

/// <summary>
/// Base class for all Page Objects.
/// Contains common navigation and utility methods.
/// </summary>
public abstract class BasePage(IPage page)
{
    protected readonly IPage Page = page;

    public async Task WaitForPageToLoad() =>
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

    public async Task TakeScreenshot(string name) =>
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = $"screenshots/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
        });
}
