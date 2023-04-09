using iHome.Scheduler.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Scheduler.Infrastructure.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSchedulesService(this IServiceCollection services)
    {
        services.AddTransient<ISchedulesService>();
        services.AddTransient<IDeviceDataService>();

        return services;
    }
}
