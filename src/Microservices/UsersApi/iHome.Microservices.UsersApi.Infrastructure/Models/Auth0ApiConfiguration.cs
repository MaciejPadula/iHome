namespace iHome.Microservices.UsersApi.Infrastructure.Models;

public class Auth0ApiConfiguration
{
    public const string Key = "Auth0";

    public string ApiToken { get; set; }
    public string ApiUrl { get; set; }
}
