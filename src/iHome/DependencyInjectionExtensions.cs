using iHome.Microservices.Devices.Contract;
using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.Widgets.Contract;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMicroservices(this IServiceCollection services)
    {
        services.AddMicroserviceClient<IDeviceManagementService>("http://172.30.0.3:5002");
        services.AddMicroserviceClient<IDeviceDataService>("http://172.30.0.3:5002");

        services.AddMicroserviceClient<IUserManagementService>("http://192.168.8.2:5003");

        services.AddMicroserviceClient<IRoomManagementService>("http://172.30.0.2:5004");
        services.AddMicroserviceClient<IRoomSharingService>("http://172.30.0.2:5004");

        services.AddMicroserviceClient<ISuggestionsService>("http://172.30.0.4:5005");

        services.AddMicroserviceClient<IWidgetManagementService>("http://192.168.8.2:5006");
        services.AddMicroserviceClient<IWidgetDeviceManagementService>("http://192.168.8.2:5006");

        services.AddMicroserviceClient<IScheduleDeviceManagementService>("http://192.168.8.2:5007");
        services.AddMicroserviceClient<IScheduleManagementService>("http://192.168.8.2:5007");



        return services;
    }
}
