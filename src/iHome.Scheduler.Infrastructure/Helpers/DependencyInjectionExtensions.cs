using iHome.Scheduler.Infrastructure.Helpers.DateTimeProvider;
using iHome.Scheduler.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Scheduler.Infrastructure.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSchedulesService(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
        services.AddTransient<ISchedulesService, SqlSchedulesService>();
        services.AddTransient<IDeviceDataService, FirebaseDeviceDataService>();

        return services;
    }
}
