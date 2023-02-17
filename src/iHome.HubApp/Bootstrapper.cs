using Microsoft.Extensions.DependencyInjection;
using System;

namespace iHome.HubApp;

public class Bootstrapper
{
    public static IServiceCollection Services { get; set; } = new ServiceCollection();
    private static IServiceProvider? _provier;

    public static T GetService<T>()
    {
        _provier ??= Services.BuildServiceProvider();
        return _provier.GetService<T>() ?? throw new Exception("Service not found");
    }
}
