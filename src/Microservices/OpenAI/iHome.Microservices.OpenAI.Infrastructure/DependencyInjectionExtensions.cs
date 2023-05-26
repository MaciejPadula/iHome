using iHome.Microservices.OpenAI.Infrastructure.Logic;
using iHome.Microservices.OpenAI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Extensions;

namespace iHome.Microservices.OpenAI.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSuggestionService(this IServiceCollection services, string? openAiKey)
    {
        services.AddOpenAIService(settings =>
        {
            settings.ApiKey = openAiKey ?? string.Empty;
        });

        services.AddScoped<IOpenAICompletions, OpenAICompletions>();
        services.AddScoped<IScheduleSuggestionsService, OpenAIScheduleSuggestionsService>();

        return services;
    }
}
