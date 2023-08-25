using iHome.Microservices.Devices.Domain.Repositories;
using iHome.Microservices.Devices.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.Devices.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDeviceRepository, DapperDeviceRepository>();
        services.AddScoped<IDeviceDataRepository, FirebaseDeviceDataRepository>();
        services.AddScoped<IDeviceDataRepository, FirestoreDeviceDataRepository>();

        return services;
    }
}
