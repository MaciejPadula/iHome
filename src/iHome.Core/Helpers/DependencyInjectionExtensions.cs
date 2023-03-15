using Firebase.Database;
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
    public static IServiceCollection AddFirebaseRepositories(this IServiceCollection services, string? url, string? authToken)
    {
        var firebaseOptions = new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(authToken),
            AsAccessToken = true
        };
        services.AddScoped(_ => new FirebaseClient(url));
        services.AddScoped<IDeviceDataRepository, FirebaseDeviceDataRepository>();

        return services;
    }

    public static IServiceCollection AddDataContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> infraBuilder)
    {
        return services.AddDbContext<SqlDataContext>(infraBuilder);
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
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
