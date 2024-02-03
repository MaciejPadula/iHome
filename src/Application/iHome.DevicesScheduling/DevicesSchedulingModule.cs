using iHome.DevicesScheduling.Features.GetDevicesForScheduling;
using iHome.DevicesScheduling.Features.GetScheduleDevices;
using iHome.DevicesScheduling.Features.ScheduleOrUpdateDevice;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Schedules.Contract;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Query;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.DevicesScheduling;

public static class DevicesSchedulingModule
{
    public static IServiceCollection AddDevicesSchedulingModule(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<ScheduleOrUpdateDeviceQuery>, ScheduleOrUpdateDeviceQueryHandler>();
        services.AddScoped<IAsyncQueryHandler<GetDevicesForSchedulingQuery>, GetDevicesForSchedulingQueryHandler>();
        services.AddScoped<IAsyncQueryHandler<GetScheduleDevicesQuery>, GetScheduleDevicesQueryHandler>();

        services.AddMicroserviceClient<IScheduleDeviceManagementService>();
        services.AddMicroserviceClient<IDeviceDataService>();
        services.AddMicroserviceClient<IDeviceManagementService>();
        return services;
    }
}
