using Newtonsoft.Json;

namespace iHome.Models.Account
{
    public class User
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
    }
}