using System.Text.Json.Serialization;

namespace AutomationLearning.Core.Models;

public record UserData(
    [property: JsonPropertyName("id")]    int    Id,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("first_name")] string FirstName,
    [property: JsonPropertyName("last_name")]  string LastName
);

public record GetUserResponse(
    [property: JsonPropertyName("data")] UserData Data
);

public record UserRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("job")]  string Job
);

public record CreateUserResponse(
    [property: JsonPropertyName("name")]      string Name,
    [property: JsonPropertyName("job")]       string Job,
    [property: JsonPropertyName("id")]        string Id,
    [property: JsonPropertyName("createdAt")] string CreatedAt
);

public record UpdateUserResponse(
    [property: JsonPropertyName("name")]      string Name,
    [property: JsonPropertyName("job")]       string Job,
    [property: JsonPropertyName("updatedAt")] string UpdatedAt
);
