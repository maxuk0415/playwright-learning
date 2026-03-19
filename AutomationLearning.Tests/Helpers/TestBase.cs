using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using AutomationLearning.Tests.Config;

namespace AutomationLearning.Tests.Helpers;

/// <summary>
/// Base class for all UI tests.
/// Handles browser setup/teardown and provides shared settings.
/// </summary>
public class TestBase : PageTest
{
    protected TestSettings Settings { get; private set; } = new();

    [SetUp]
    public void LoadSettings()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        Settings = config.Get<TestSettings>() ?? new TestSettings();
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 },
            RecordVideoDir = "videos/"
        };
    }
}
