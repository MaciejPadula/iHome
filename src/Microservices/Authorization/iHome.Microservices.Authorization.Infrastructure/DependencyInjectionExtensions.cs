using iHome.Microservices.Authorization.Domain.Repositories;
using iHome.Microservices.Authorization.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.Authorization.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRoomRepository, DapperRoomRepository>();
        services.AddScoped<IDeviceRepository, DapperDeviceRepository>();
        services.AddScoped<IScheduleRepostitory, DapperScheduleRepostitory>();
        services.AddScoped<IWidgetRepository, DapperWidgetRepository>();

        return services;
    }
}
