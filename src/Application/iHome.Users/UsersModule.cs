using iHome.Microservices.UsersApi.Contract;
using iHome.Users.Features.GetUsers;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Query;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.Suggestions;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<GetUsersQuery>, GetUsersQueryHandler>();
        services.AddMicroserviceClient<IUserManagementService>();
        return services;
    }
}
