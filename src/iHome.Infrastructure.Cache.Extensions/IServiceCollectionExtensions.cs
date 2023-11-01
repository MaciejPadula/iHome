using iHome.Infrastructure.Cache.Logic;
using iHome.Infrastructure.Cache.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Infrastructure.Cache.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCacheByStrategy(this IServiceCollection services, CacheStrategy strategy, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var strategyType = strategy switch
        {
            CacheStrategy.Memory => typeof(MemoryCacheStrategy),
            CacheStrategy.Distributed => typeof(DistributedCacheStrategy),
            _ => typeof(MemoryCacheStrategy)
        };

        services.Add(new ServiceDescriptor(
            typeof(ICache),
            strategyType,
            lifetime));

        return services;
    }

    public static IServiceCollection AddCacheFetcher(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(
            typeof(ICacheFetcher),
            typeof(CacheFetcher),
            lifetime));

        return services;
    }
}
