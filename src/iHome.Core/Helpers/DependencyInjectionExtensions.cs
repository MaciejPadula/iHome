using iHome.Core.Logic.AccessGuards;
using iHome.Core.Logic.ActionValidators;
using iHome.Core.Logic.Providers;
using iHome.Core.Repositories.Devices;
using iHome.Core.Repositories.Schedules;
using iHome.Core.Repositories.Widgets;
using iHome.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IDeviceProvider, EFDeviceProvider>();
        services.AddAccessGuards();
        services.AddActionValidators();
        services.AddRepositories();

        services.AddScoped<IScheduleService, ScheduleService>();

        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IDevicesForSchedulingAccessor, DevicesForSchedulingAccessor>();

        services.AddScoped<IWidgetService, WidgetService>();

        return services;
    }

    private static IServiceCollection AddAccessGuards(this IServiceCollection services)
    {
        services.AddTransient<IDeviceAccessGuard, EFDeviceAccessGuard>();
        services.AddTransient<IRoomAccessGuard, EFRoomAccessGuard>();
        return services;
    }

    private static IServiceCollection AddActionValidators(this IServiceCollection services)
    {
        services.AddTransient<IDeviceActionValidator, DeviceActionValidator>();
        services.AddTransient<IRoomActionValidator, RoomActionValidator>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDeviceRepository, EFDeviceRepository>();
        services.AddScoped<IScheduleRepository, EFScheduleRepository>();
        services.AddScoped<IScheduleDeviceRepository, EFScheduleDeviceRepository>();
        services.AddScoped<IWidgetRepository, EFWidgetRepository>();
        services.AddScoped<IWidgetDeviceRepository, EFWidgetDeviceRepository>();
        return services;
    }
}
