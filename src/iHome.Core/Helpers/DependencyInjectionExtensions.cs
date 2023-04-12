using Firebase.Database;
using iHome.Core.Models;
using iHome.Core.Services.Devices;
using iHome.Core.Services.Rooms;
using iHome.Core.Services.Schedules;
using iHome.Core.Services.Users;
using iHome.Core.Services.Widgets;
using iHome.Shared.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Core.Helpers;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IWidgetService, WidgetService>();

        return services;
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string? token)
    {
        services.AddScoped(_ => new Auth0ApiConfiguration { Token = token ?? string.Empty });
        services.AddScoped<IUserService, Auth0UserService>();
        
        return services;
    }
}
