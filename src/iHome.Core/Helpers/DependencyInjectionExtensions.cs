using iHome.Core.Repositories;
using iHome.Core.Services.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> infraBuilder, Action<DbContextOptionsBuilder> devicesBuilder)
    {
        return services
            .AddDbContext<InfraDataContext>(infraBuilder)
            .AddDbContext<DevicesDataContext>(devicesBuilder);
    }

    public static IServiceCollection AddRoomService(this IServiceCollection services)
    {
        return services
            .AddScoped<IRepository, CachedRepository>()
            .AddScoped<IRoomService, RoomService>();
    }
}
