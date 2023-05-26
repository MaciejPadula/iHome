using iHome.Jobs.Events.Infrastructure.Helpers.DateTimeProvider;
using iHome.Jobs.Events.Infrastructure.Services.SchedulesService;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Jobs.Events.Infrastructure.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSchedulesService(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
        services.AddTransient<ISchedulesService, SqlSchedulesService>();

        return services;
    }
}
