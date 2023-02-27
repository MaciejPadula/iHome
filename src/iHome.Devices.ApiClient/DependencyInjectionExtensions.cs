using iHome.Devices.Contract.Interfaces;
using iHome.Shared.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace iHome.Devices.ApiClient;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureApiClient(this IServiceCollection services, string settingsFile)
    {
        try
        {
            var text = File.ReadAllText(settingsFile);
            var settings = JsonConvert.DeserializeObject<ApiClientSettings>(text);
            if (settings == null) return services;

            services.TryAddScoped(_ => settings);
        }
        catch { }
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
