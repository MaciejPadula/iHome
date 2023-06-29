namespace iHome.Microservices.UsersApi.Infrastructure.Models;

public class Auth0ApiConfiguration
{
    public const string Key = "Auth0";

    public required string ApiToken { get; set; }
}
