using iHome.Shared.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Shared;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, HttpContextUserAccessor>();
        services.AddTransient<ITimeModelParser, TimeModelParser>();
        return services;
    }
}
