using iHome.Devices.Contract.Interfaces;
using iHome.Shared.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Devices.ApiClient;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureApiClient(this IServiceCollection services, string? baseApiUrl)
    {
        services.TryAddScoped(_ => new ApiClientSettings
        {
            BaseApiUrl = baseApiUrl ?? string.Empty
        });
        return services;
    }

    public static IServiceCollection AddDeviceManipulator(this IServiceCollection services)
    {
        services.TryAddScoped<JsonHttpClient>();
        return services.AddScoped<IDeviceManipulator, HttpDeviceManipulator>();
    }

    public static IServiceCollection AddDeviceProvider(this IServiceCollection services)
    {
        services.TryAddScoped<JsonHttpClient>();
        return services.AddScoped<IDeviceProvider, HttpDeviceProvider>();
    }
}
