using Newtonsoft.Json;

namespace iHome.Core.Models;

public class User
{
    [JsonProperty("user_id")]
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
