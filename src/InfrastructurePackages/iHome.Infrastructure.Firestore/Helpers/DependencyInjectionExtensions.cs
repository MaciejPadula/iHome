using iHome.Infrastructure.Firestore.Serializers;
using iHome.Microservices.Devices.Infrastructure.Logic;
using iHome.Microservices.Devices.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Infrastructure.Firestore.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFirestoreConnectionFactory(this IServiceCollection services, Action<FirestoreOptions> optionsPredicate)
    {
        var options = new FirestoreOptions();
        optionsPredicate(options);

        services.AddTransient<IMessageSerializer, NewtonsoftMessageSerializer>();
        services.AddScoped<IFirestoreConnectionFactory>(provider => new FirestoreConnectionFactory(
            options, provider.GetRequiredService<IMessageSerializer>()));

        return services;
    }
}
