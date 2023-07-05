using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Infrastructure.Queue.Service.Write;
using iHome.Infrastructure.Queue.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Infrastructure.Queue.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddQueueReader<T>(
        this IServiceCollection services,
        Action<QueueOptions<T>> optionsPredicate,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.TryAddQueueClientWrapper(CreateOptions(optionsPredicate));
        services.TryAdd<IQueueReader<T>, AzureQueueReader<T>>(serviceLifetime);
        return services;
    }

    public static IServiceCollection AddQueueWriter<T>(
        this IServiceCollection services,
        Action<QueueOptions<T>> optionsPredicate,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.TryAddQueueClientWrapper(CreateOptions(optionsPredicate));
        services.TryAdd<IQueueWriter<T>, AzureQueueWriter<T>>(serviceLifetime);
        return services;
    }

    public static IServiceCollection AddQueueFullAccess<T>(
        this IServiceCollection services,
        Action<QueueOptions<T>> optionsPredicate,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddQueueReader(optionsPredicate, serviceLifetime);
        services.AddQueueWriter(optionsPredicate, serviceLifetime);
        services.TryAdd<IQueueFullAccess<T>, QueueFullAccess<T>>(serviceLifetime);
        return services;
    }

    private static void TryAddQueueClientWrapper<T>(
        this IServiceCollection services,
        QueueOptions<T> options)
    {
        services.TryAddSingleton(options);
        services.TryAddScoped<IQueueClientWrapper<T>, QueueClientWrapper<T>>();
    }

    private static IServiceCollection TryAdd<TInterface, TClass>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime)
        where TClass : TInterface
    {
        services.TryAdd(new ServiceDescriptor(
            typeof(TInterface),
            typeof(TClass),
            serviceLifetime));

        return services;
    }

    private static QueueOptions<T> CreateOptions<T>(Action<QueueOptions<T>> optionsPredicate)
    {
        var options = new QueueOptions<T>();
        optionsPredicate(options);
        return options;
    }
}
