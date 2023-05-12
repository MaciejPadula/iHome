using iHome.Infrastructure.OpenAI.Logic;
using iHome.Infrastructure.OpenAI.Services.Suggestions;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Extensions;

namespace iHome.Infrastructure.OpenAI.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSuggestionService(this IServiceCollection services, string? openAiKey)
    {
        services.AddOpenAIService(settings => 
        {
            settings.ApiKey = openAiKey ?? string.Empty; 
        });

        services.AddScoped<IOpenAICompletions, OpenAICompletions>();
        services.AddScoped<ISuggestionsService, OpenAISuggestionsService>();

        return services;
    }
}
