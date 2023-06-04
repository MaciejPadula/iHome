using iHome.Microservices.Devices.Contract;
using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.Widgets.Contract;
using Web.Infrastructure.Microservices.Client.Configuration.Extensions;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMicroservices(this IServiceCollection services)
    {
        services.AddConfigurationServiceLookup("Microservices");

        services.AddMicroserviceClient<IDeviceManagementService>();
        services.AddMicroserviceClient<IDeviceDataService>();

        services.AddMicroserviceClient<IUserManagementService>();

        services.AddMicroserviceClient<IRoomManagementService>();
        services.AddMicroserviceClient<IRoomSharingService>();

        services.AddMicroserviceClient<ISuggestionsService>();

        services.AddMicroserviceClient<IWidgetManagementService>();
        services.AddMicroserviceClient<IWidgetDeviceManagementService>();

        services.AddMicroserviceClient<IScheduleDeviceManagementService>();
        services.AddMicroserviceClient<IScheduleManagementService>();

        return services;
    }
}
