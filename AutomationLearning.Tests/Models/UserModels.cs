using System.Text.Json.Serialization;

namespace AutomationLearning.Tests.Models;

/// <summary>
/// Maps to the "data" object inside GET /api/users/{id} response.
/// Postman equivalent: pm.response.json().data
/// </summary>
public record UserData(
    [property: JsonPropertyName("id")]    int    Id,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("first_name")] string FirstName,
    [property: JsonPropertyName("last_name")]  string LastName
);

/// <summary>
/// Maps to GET /api/users/{id} full response body.
/// Postman equivalent: pm.response.json()
/// </summary>
public record GetUserResponse(
    [property: JsonPropertyName("data")] UserData Data
);

/// <summary>
/// Request body for POST /api/users and PUT /api/users/{id}.
/// Postman equivalent: request body (raw JSON)
/// </summary>
public record UserRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("job")]  string Job
);

/// <summary>
/// Maps to POST /api/users response body.
/// Postman equivalent: pm.response.json()
/// </summary>
public record CreateUserResponse(
    [property: JsonPropertyName("name")]      string Name,
    [property: JsonPropertyName("job")]       string Job,
    [property: JsonPropertyName("id")]        string Id,
    [property: JsonPropertyName("createdAt")] string CreatedAt
);

/// <summary>
/// Maps to PUT /api/users/{id} response body.
/// </summary>
public record UpdateUserResponse(
    [property: JsonPropertyName("name")]      string Name,
    [property: JsonPropertyName("job")]       string Job,
    [property: JsonPropertyName("updatedAt")] string UpdatedAt
);
