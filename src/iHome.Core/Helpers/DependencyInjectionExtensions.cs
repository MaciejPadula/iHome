using iHome.Core.Repositories.Schedules;
using iHome.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IScheduleRepository, EFScheduleRepository>();
        services.AddScoped<IScheduleDeviceRepository, EFScheduleDeviceRepository>();
        services.AddScoped<IScheduleService, ScheduleService>();

        services.AddScoped<IDevicesForSchedulingAccessor, DevicesForSchedulingAccessor>();

        return services;
    }
}
