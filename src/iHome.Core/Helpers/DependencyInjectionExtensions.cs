using iHome.Core.Repositories;
using iHome.Core.Services.Devices;
using iHome.Core.Services.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> infraBuilder)
    {
        return services
            .AddDbContext<SqlDataContext>(infraBuilder);
    }

    public static IServiceCollection AddRoomService(this IServiceCollection services)
    {
        return services.AddScoped<IRoomService, RoomService>();
    }

    public static IServiceCollection AddDeviceService(this IServiceCollection services)
    {
        return services.AddScoped<IDeviceService, DeviceService>();
    }
}
