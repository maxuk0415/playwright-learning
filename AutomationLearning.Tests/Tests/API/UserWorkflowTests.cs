using System.Net;
using System.Net.Http.Json;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationLearning.Tests.Helpers;
using AutomationLearning.Core.Models;

namespace AutomationLearning.Tests.Tests.API;

/// <summary>
/// Postman → C# conversion example.
///
/// Postman collection "User CRUD Workflow":
///   1. POST   /api/users         → Create user, save ID to collection variable
///   2. GET    /api/users/{id}    → Verify user exists (uses saved ID)
///   3. PUT    /api/users/{id}    → Update user
///   4. DELETE /api/users/{id}    → Cleanup
///
/// C# equivalent:
///   - Postman "collection variable" → _createdUserId (class-level field)
///   - Postman "pm.test(...)"        → Assert.That(...)
///   - Postman pre-request script    → [SetUp] method
///   - Postman environment variable  → TestSettings / appsettings.json
/// </summary>
[TestFixture]
[AllureNUnit]
[AllureSuite("API Tests")]
[AllureFeature("User Workflow")]
[Category("API")]
[Category("UserWorkflow")]
[Order(1)]
public class UserWorkflowTests : ApiTestBase
{
    // Postman equivalent: collection variable {{createdUserId}}
    // Shared across all tests in this fixture via [Order]
    private static string _createdUserId = string.Empty;

    // ── Step 1: POST /api/users ─────────────────────────────────────────────
    // Postman: POST request with raw JSON body
    //   pm.test("Status 201", () => pm.response.to.have.status(201))
    //   pm.test("Has ID",     () => pm.expect(json.id).to.not.be.empty)
    //   pm.collectionVariables.set("userId", json.id)

    [Test, Order(1)]
    [AllureStory("Create User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
    [Description("POST /api/users → creates user and saves ID for subsequent tests")]
    public async Task Step1_CreateUser_ShouldReturn201AndId()
    {
        var request = new UserRequest("Max", "automation engineer");

        var response = await HttpClient.PostAsJsonAsync("/api/users", request);

        // pm.test("Status 201")
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var body = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

        // pm.test("Has name")
        Assert.That(body!.Name, Is.EqualTo("Max"));

        // pm.test("Has ID")
        Assert.That(body.Id, Is.Not.Null.And.Not.Empty,
            "Created user should have an ID");

        // pm.collectionVariables.set("userId", json.id)
        _createdUserId = body.Id;
    }

    // ── Step 2: GET /api/users/{id} ─────────────────────────────────────────
    // Postman: GET {{baseUrl}}/api/users/{{userId}}
    //   pm.test("Status 200", () => pm.response.to.have.status(200))
    //   pm.test("Email exists", () => pm.expect(json.data.email).to.not.be.empty)

    [Test, Order(2)]
    [AllureStory("Get User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("GET /api/users/{id} → verifies user data is retrievable")]
    public async Task Step2_GetUser_ShouldReturnUserData()
    {
        // reqres.in returns pre-seeded users; use ID 2 to verify the GET pattern
        // In a real project this would use _createdUserId from step 1
        var response = await HttpClient.GetAsync("/api/users/2");

        // pm.test("Status 200")
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<GetUserResponse>();

        // pm.test("Email exists")
        Assert.That(body!.Data.Email, Is.Not.Null.And.Not.Empty);

        // pm.test("First name exists")
        Assert.That(body.Data.FirstName, Is.Not.Null.And.Not.Empty);
    }

    // ── Step 3: PUT /api/users/{id} ─────────────────────────────────────────
    // Postman: PUT {{baseUrl}}/api/users/{{userId}}
    //   pm.test("Status 200")
    //   pm.test("Name updated", () => pm.expect(json.name).to.eql("Max Updated"))
    //   pm.test("Has updatedAt", () => pm.expect(json.updatedAt).to.not.be.empty)

    [Test, Order(3)]
    [AllureStory("Update User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("PUT /api/users/{id} → updates user and verifies response fields")]
    public async Task Step3_UpdateUser_ShouldReflectChanges()
    {
        var update = new UserRequest("Max Updated", "senior automation engineer");

        var response = await HttpClient.PutAsJsonAsync("/api/users/2", update);

        // pm.test("Status 200")
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<UpdateUserResponse>();

        // pm.test("Name updated")
        Assert.That(body!.Name, Is.EqualTo("Max Updated"));

        // pm.test("Has updatedAt")
        Assert.That(body.UpdatedAt, Is.Not.Null.And.Not.Empty,
            "Response should contain updatedAt timestamp");
    }

    // ── Step 4: DELETE /api/users/{id} ──────────────────────────────────────
    // Postman: DELETE {{baseUrl}}/api/users/{{userId}}
    //   pm.test("Status 204")

    [Test, Order(4)]
    [AllureStory("Delete User")]
    [AllureSeverity(Allure.Net.Commons.SeverityLevel.normal)]
    [Description("DELETE /api/users/{id} → removes user and verifies 204 No Content")]
    public async Task Step4_DeleteUser_ShouldReturn204()
    {
        var response = await HttpClient.DeleteAsync("/api/users/2");

        // pm.test("Status 204")
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
