using iHome.Microservices.OpenAI.Contract;
using iHome.Suggestions.Features.GetSuggestedDevices;
using iHome.Suggestions.Features.GetSuggestedTime;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Query;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.Suggestions;

public static class SuggestionsModule
{
    public static IServiceCollection AddSuggestionsModule(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<GetSuggestedDevicesQuery>, GetSuggestedDevicesQueryHandler>();
        services.AddScoped<IAsyncQueryHandler<GetSuggestedTimeQuery>, GetSuggestedTimeQueryHandler>();
        services.AddMicroserviceClient<ISuggestionsService>();
        return services;
    }
}
