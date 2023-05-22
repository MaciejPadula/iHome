using iHome.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IDevicesForSchedulingAccessor, DevicesForSchedulingAccessor>();

        return services;
    }
}
