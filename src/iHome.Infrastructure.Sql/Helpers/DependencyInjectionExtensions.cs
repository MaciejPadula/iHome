using iHome.Infrastructure.Sql.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Infrastructure.Sql.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDbConnectionFactory(this IServiceCollection services, string connectionString)
    {
        services.TryAddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        return services;
    }
}
