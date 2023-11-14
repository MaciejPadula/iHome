using iHome.Features.AddSchedule.Service;
using iHome.Features.AddSchedule.Service.AddSchedule;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.AddSchedule;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddScheduleConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IAsyncCommandHandler<AddScheduleCommand>, AddScheduleCommandHandler>();
        services.AddScoped<IAddScheduleService, AddScheduleService>();
        return services;
    }
}
