using Microsoft.Extensions.Configuration;
using AutomationLearning.Core.Config;

namespace AutomationLearning.Tests.Helpers;

/// <summary>
/// Base class for all API tests.
/// Provides a shared HttpClient and settings.
/// </summary>
public class ApiTestBase
{
    protected HttpClient HttpClient { get; private set; } = null!;
    protected TestSettings Settings { get; private set; } = new();

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        Settings = config.Get<TestSettings>() ?? new TestSettings();

        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(Settings.ApiBaseUrl)
        };
        HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        if (!string.IsNullOrEmpty(Settings.ApiKey))
            HttpClient.DefaultRequestHeaders.Add("x-api-key", Settings.ApiKey);
    }

    [TearDown]
    public void TearDown()
    {
        HttpClient.Dispose();
    }
}
