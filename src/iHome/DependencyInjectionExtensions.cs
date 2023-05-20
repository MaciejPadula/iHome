using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.Widgets.Contract;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMicroservices(this IServiceCollection services)
    {
        services.AddMicroserviceClient<IRoomManagementService>("https://localhost:7019");
        services.AddMicroserviceClient<IRoomSharingService>("https://localhost:7019");

        services.AddMicroserviceClient<IWidgetManagementService>("https://localhost:7206");
        services.AddMicroserviceClient<IWidgetDeviceManagementService>("https://localhost:7206");

        services.AddMicroserviceClient<ISuggestionsService>("https://localhost:7018");

        services.AddMicroserviceClient<IUserManagementService>("https://localhost:7094");

        return services;
    }
}
