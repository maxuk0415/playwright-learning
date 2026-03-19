using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationLearning.Tests.Helpers;
using AutomationLearning.Tests.Pages;

namespace AutomationLearning.Tests.Tests.UI;

[TestFixture]
[AllureNUnit]
[AllureSuite("UI Tests")]
[AllureFeature("Authentication")]
[Category("UI")]
[Category("Login")]
public class LoginTests : TestBase
{
    private LoginPage _loginPage = null!;
    private InventoryPage _inventoryPage = null!;

    [SetUp]
    public void SetUpPages()
    {
        _loginPage = new LoginPage(Page);
        _inventoryPage = new InventoryPage(Page);
    }

    [Test]
    [AllureStory("Valid Login")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
    [Description("Valid user should be redirected to inventory page after login")]
    public async Task Login_WithValidCredentials_ShouldNavigateToInventory()
    {
        await _loginPage.GoTo(Settings.BaseUrl);
        await _loginPage.Login(Settings.ValidUsername, Settings.ValidPassword);

        Assert.That(await _inventoryPage.IsLoaded(), Is.True,
            "Inventory page should be visible after successful login");
        Assert.That(Page.Url, Does.Contain("inventory.html"),
            "URL should contain 'inventory.html'");
    }

    [Test]
    [AllureStory("Invalid Login")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("Locked out user should see an error message")]
    public async Task Login_WithLockedOutUser_ShouldShowError()
    {
        await _loginPage.GoTo(Settings.BaseUrl);
        await _loginPage.Login(Settings.LockedUsername, Settings.ValidPassword);

        Assert.That(await _loginPage.IsErrorVisible(), Is.True,
            "Error message should be visible");
        Assert.That(await _loginPage.GetErrorMessage(),
            Does.Contain("locked out"),
            "Error should mention account is locked out");
    }

    [Test]
    [AllureStory("Invalid Login")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("Wrong password should show invalid credentials error")]
    public async Task Login_WithWrongPassword_ShouldShowError()
    {
        await _loginPage.GoTo(Settings.BaseUrl);
        await _loginPage.Login(Settings.ValidUsername, "wrong_password");

        Assert.That(await _loginPage.IsErrorVisible(), Is.True,
            "Error message should be visible");
    }

    [Test]
    [AllureStory("Invalid Login")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.minor)]
    [Description("Empty username should show validation error")]
    public async Task Login_WithEmptyUsername_ShouldShowError()
    {
        await _loginPage.GoTo(Settings.BaseUrl);
        await _loginPage.Login("", Settings.ValidPassword);

        Assert.That(await _loginPage.IsErrorVisible(), Is.True,
            "Error should appear for empty username");
    }
}
