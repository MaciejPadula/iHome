using Auth0.OidcClient;
using iHome.HubApp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.HubApp.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuth0Configuration(this IServiceCollection services, string? audience)
    {
        return services
            .AddScoped(_ => new Auth0ClientOptions
            {
                Domain = "dev-e7eyj4xg.eu.auth0.com",
                ClientId = "gRbUysKvbUB49XCU9y3RlOmoYFFmVHWJ"
            })
            .AddScoped(_ => new Auth0ServiceConfiguration
            {
                Audience = audience ?? string.Empty
            });
    }
}
