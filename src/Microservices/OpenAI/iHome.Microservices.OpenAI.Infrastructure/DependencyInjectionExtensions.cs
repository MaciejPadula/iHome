using iHome.Infrastructure.Cache.Extensions;
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
        services.AddOpenAIService(settings =>
        {
            settings.ApiKey = openAiKey ?? string.Empty;
        });

        services.AddScoped<IOpenAICompletions, OpenAICompletions>();
        services.AddCachedRepository<IScheduleSuggestionsProvider, OpenAIScheduleSuggestionsProvider>(
            p => new OpenAIScheduleSuggestionsProvider(p.GetRequiredService<IOpenAICompletions>()),
            (c, impl) => new CachedScheduleSuggestionsProvider(impl, c));

        return services;
    }
}
