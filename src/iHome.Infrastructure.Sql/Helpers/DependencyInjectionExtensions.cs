using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Infrastructure.Sql.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDbConnectionFactory(
        this IServiceCollection services,
        Action<DbConnectionFactoryOptions> optionsPredicate,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var options = new DbConnectionFactoryOptions();
        optionsPredicate(options);

        var descriptor = new ServiceDescriptor(
            typeof(IDbConnectionFactory),
            p => new DbConnectionFactory(options),
            lifetime
        );

        services.TryAdd(descriptor);
        return services;
    }
}
