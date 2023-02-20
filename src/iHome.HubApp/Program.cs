using Auth0.OidcClient;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using iHome.HubApp.Logic.ClaimsResolver;
using iHome.HubApp.Services.UserService;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace iHome.HubApp;
internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Bootstrapper.Services
            .AddScoped<IClaimsResolver, ClaimsResolver>()
            .AddScoped(_ => new Auth0ClientOptions()
            {
                Domain = "dev-e7eyj4xg.eu.auth0.com",
                ClientId = ""
            })
            .AddScoped<Auth0Client>()
            .AddScoped<IUserService, Auth0UserService>();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
