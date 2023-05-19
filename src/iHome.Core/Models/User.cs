using Newtonsoft.Json;

namespace iHome.Core.Models;

public class User
{
    [JsonProperty("user_id")]
    public required string Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}
