using System.Text.Json.Serialization;

namespace iHome.Microservices.UsersApi.Infrastructure.Models;

public class UserResponse
{
    [JsonPropertyName("user_id")]
    public required string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;
}
