using iHome.Core.Repositories.Widgets;
using iHome.Microservices.Widgets.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.Widgets.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IWidgetRepository, DapperWidgetRepository>();
        services.AddScoped<IWidgetDeviceRepository, DapperWidgetDeviceRepository>();
        services.AddScoped<IDbConnectionFactory, SqlDbConnectionFactory>();

        return services;
    }
}
