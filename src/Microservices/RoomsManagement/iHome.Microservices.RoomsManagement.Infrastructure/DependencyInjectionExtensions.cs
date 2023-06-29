using iHome.Microservices.RoomsManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.RoomsManagement.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRoomRepository, DapperRoomRepository>();
        services.AddScoped<IUserRoomRepository, DapperUserRoomRepository>();

        return services;
    }
}
