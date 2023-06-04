using iHome.Microservices.Schedules.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.Schedules.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IScheduleRepository, DapperScheduleRepository>();
        services.AddScoped<IScheduleDeviceRepository, DapperScheduleDeviceRepository>();
        services.AddScoped<IScheduleRunHistoryRepository, DapperScheduleRunHistoryRepository>();
        services.AddScoped<IDbConnectionFactory, SqlDbConnectionFactory>();

        return services;
    }
}
