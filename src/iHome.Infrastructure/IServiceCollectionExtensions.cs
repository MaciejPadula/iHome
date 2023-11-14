using iHome.Infrastructure.Repository;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.Widgets.Contract;
using iHome.Repository;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Microservices.Client.Builders;
using Web.Infrastructure.Microservices.Client.Configuration.Extensions;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoomSharesRepository, RoomSharesRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddMicroservices();
        return services;
    }

    public static IServiceCollection AddMicroservices(this IServiceCollection services)
    {
        services.AddConfigurationServiceLookup("Microservices");

        services.AddLongRunningMicroservices<IDeviceManagementService>();
        services.AddLongRunningMicroservices<IDeviceDataService>();

        services.AddLongRunningMicroservices<IUserManagementService>();

        services.AddLongRunningMicroservices<IRoomManagementService>();
        services.AddLongRunningMicroservices<IRoomSharingService>();

        services.AddLongRunningMicroservices<ISuggestionsService>();

        services.AddLongRunningMicroservices<IWidgetManagementService>();
        services.AddLongRunningMicroservices<IWidgetDeviceManagementService>();

        services.AddLongRunningMicroservices<IScheduleDeviceManagementService>();
        services.AddLongRunningMicroservices<IScheduleManagementService>();

        services.AddLongRunningMicroservices<IRoomAuthService>();
        services.AddLongRunningMicroservices<IDeviceAuthService>();
        services.AddLongRunningMicroservices<IWidgetAuthService>();
        services.AddLongRunningMicroservices<IScheduleAuthService>();

        return services;
    }

    private static IServiceCollection AddLongRunningMicroservices<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddMicroserviceClient<T>(_ => { },
            MicroserviceClientConfigurationBuilder.GetLongRunningConfig());
    }
}
