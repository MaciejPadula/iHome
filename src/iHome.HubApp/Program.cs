using Auth0.OidcClient;
using Avalonia;
using Avalonia.ReactiveUI;
using iHome.Devices.ApiClient;
using iHome.HubApp.Helpers;
using iHome.HubApp.Logic.ClaimsResolver;
using iHome.HubApp.Services.UserService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace iHome.HubApp;
internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("settings.json")
            .Build();

        Bootstrapper.Services
            .AddScoped<IClaimsResolver, ClaimsResolver>()
            .AddAuth0Configuration(configuration["Audience"])
            .AddScoped<IAuth0Client, Auth0Client>()
            .AddScoped<IUserService, Auth0UserService>()
            .ConfigureApiClient(configuration["BaseApiUrl"])
            .AddDeviceProvider()
            .AddDeviceManipulator();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
