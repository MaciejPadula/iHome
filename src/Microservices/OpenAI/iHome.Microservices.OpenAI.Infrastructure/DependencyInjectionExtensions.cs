using iHome.Microservices.OpenAI.Infrastructure.Logic;
using iHome.Microservices.OpenAI.Infrastructure.Providers;
using iHome.Microservices.OpenAI.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;

namespace iHome.Microservices.OpenAI.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSuggestionsProviders(this IServiceCollection services, string? openAiKey, bool useCache = true)
    {
        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddOpenAIService(settings =>
        {
            settings.ApiKey = openAiKey ?? string.Empty;
        });

        services.AddScoped<IOpenAICompletions, OpenAICompletions>();
        services.AddScheduleSuggestionsProvider(useCache);

        return services;
    }

    private static IServiceCollection AddCachedService<I>(
        this IServiceCollection services,
        Func<IServiceProvider, I, I> cacheFactory,
        Func<IServiceProvider, I> implementationFactory)
        where I : class
    {
        return services.AddScoped(p =>
        {
            var implemenation = implementationFactory(p);
            return cacheFactory(p, implemenation);
        });
    }

    private static IServiceCollection AddScheduleSuggestionsProvider(this IServiceCollection services, bool useCache)
    {
        if (!useCache)
        {
            return services.AddScoped<IScheduleSuggestionsProvider, OpenAIScheduleSuggestionsProvider>();
        }

        return services.AddCachedService<IScheduleSuggestionsProvider>(
            (p, impl) => new CachedScheduleSuggestionsProvider(impl, p.GetRequiredService<IMemoryCache>()),
            p => new OpenAIScheduleSuggestionsProvider(p.GetRequiredService<IOpenAICompletions>()));
    }
}
