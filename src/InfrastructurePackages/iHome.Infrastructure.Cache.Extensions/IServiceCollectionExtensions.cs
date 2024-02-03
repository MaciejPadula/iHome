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

    public static IServiceCollection AddCachedRepository<TInterface, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> implementationFactory,
        Func<ICache, TInterface, TInterface> cacheFactory,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TImplementation : TInterface
        where TInterface : class
    {
        services.Add(new ServiceDescriptor(
            typeof(TInterface),
            provider =>
            {
                var service = implementationFactory(provider);
                var cache = provider.GetRequiredService<ICache>();
                return cacheFactory(cache, service);
            },
            lifetime));

        return services;
    }
}
