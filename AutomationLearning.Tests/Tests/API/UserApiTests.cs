using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationLearning.Tests.Helpers;

namespace AutomationLearning.Tests.Tests.API;

[TestFixture]
[AllureNUnit]
[AllureSuite("API Tests")]
[AllureFeature("User API")]
[Category("API")]
[Category("Users")]
public class UserApiTests : ApiTestBase
{
    // --- GET Tests ---

    [Test]
    [AllureStory("Get Users")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
    [Description("GET /api/users should return 200 and a list of users")]
    public async Task GetUsers_ShouldReturn200WithUserList()
    {
        var response = await HttpClient.GetAsync("/api/users?page=1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.That(body.GetProperty("data").GetArrayLength(), Is.GreaterThan(0),
            "Response should contain at least one user");
    }

    [Test]
    [AllureStory("Get Users")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("GET /api/users/{id} should return a specific user")]
    public async Task GetUser_WithValidId_ShouldReturnUserData()
    {
        var response = await HttpClient.GetAsync("/api/users/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var user = body.GetProperty("data");

        Assert.That(user.GetProperty("id").GetInt32(), Is.EqualTo(2));
        Assert.That(user.GetProperty("email").GetString(), Is.Not.Null.Or.Empty);
    }

    [Test]
    [AllureStory("Get Users")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("GET /api/users/{id} with invalid id should return 404")]
    public async Task GetUser_WithInvalidId_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync("/api/users/999");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    // --- POST Tests ---

    [Test]
    [AllureStory("Create User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
    [Description("POST /api/users should create a user and return 201")]
    public async Task CreateUser_WithValidData_ShouldReturn201()
    {
        var newUser = new { name = "Max", job = "automation engineer" };
        var response = await HttpClient.PostAsJsonAsync("/api/users", newUser);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.That(body.GetProperty("name").GetString(), Is.EqualTo("Max"));
        Assert.That(body.GetProperty("id").GetString(), Is.Not.Null.Or.Empty,
            "Created user should have an ID");
    }

    // --- PUT Tests ---

    [Test]
    [AllureStory("Update User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("PUT /api/users/{id} should update user and return 200")]
    public async Task UpdateUser_WithValidData_ShouldReturn200()
    {
        var update = new { name = "Max Updated", job = "senior automation engineer" };
        var response = await HttpClient.PutAsJsonAsync("/api/users/2", update);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.That(body.GetProperty("name").GetString(), Is.EqualTo("Max Updated"));
        Assert.That(body.TryGetProperty("updatedAt", out _), Is.True,
            "Response should contain updatedAt timestamp");
    }

    // --- DELETE Tests ---

    [Test]
    [AllureStory("Delete User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("DELETE /api/users/{id} should return 204 No Content")]
    public async Task DeleteUser_ShouldReturn204()
    {
        var response = await HttpClient.DeleteAsync("/api/users/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
