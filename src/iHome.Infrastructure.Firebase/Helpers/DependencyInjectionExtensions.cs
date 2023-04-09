using Firebase.Database;
using iHome.Infrastructure.Firebase.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Infrastructure.Firebase.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFirebaseRepositories(this IServiceCollection services, string? url, string? authToken)
    {
        var firebaseOptions = new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(authToken),
            AsAccessToken = true
        };
        services.AddScoped(_ => new FirebaseClient(url, firebaseOptions));
        services.AddScoped<IDeviceDataRepository, FirebaseDeviceDataRepository>();

        return services;
    }
}
