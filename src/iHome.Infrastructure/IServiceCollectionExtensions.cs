using iHome.Infrastructure.Repository;
using iHome.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoomSharesRepository, RoomSharesRepository>();
        return services;
    }
}
