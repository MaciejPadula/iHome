using iHome.Microservices.OpenAI.Infrastructure.Logic;
using iHome.Microservices.OpenAI.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;

namespace iHome.Microservices.OpenAI.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSuggestionService(this IServiceCollection services, string? openAiKey)
    {
        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddOpenAIService(settings =>
        {
            settings.ApiKey = openAiKey ?? string.Empty;
        });

        services.AddScoped<IOpenAICompletions, OpenAICompletions>();
        services.AddScoped<IScheduleSuggestionsService>(provider =>
        {
            var cache = provider.GetRequiredService<IMemoryCache>();
            var scheduleServiceImplementation = new OpenAIScheduleSuggestionsService(provider.GetRequiredService<IOpenAICompletions>());

            return new CachedScheduleSuggestionsService(scheduleServiceImplementation, cache);
        });

        return services;
    }
}
