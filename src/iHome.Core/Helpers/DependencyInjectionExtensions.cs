using iHome.Core.Models;
using iHome.Core.Repositories;
using iHome.Core.Services.Devices;
using iHome.Core.Services.Rooms;
using iHome.Core.Services.Users;
using iHome.Core.Services.Widgets;
using iHome.Shared.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

    public static IServiceCollection AddWidgetService(this IServiceCollection services)
    {
        return services.AddScoped<IWidgetService, WidgetService>();
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string? token)
    {
        services
            .AddScoped(_ => new Auth0ApiConfiguration { Token = token ?? string.Empty })
            .AddScoped<IUserService, Auth0UserService>()
            .TryAddScoped<JsonHttpClient>();

        return services;
    }
}
