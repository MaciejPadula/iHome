using iHome.Features.SchedulesList.Service;
using iHome.Features.SchedulesList.Service.GetUserSchedulesOrdered;
using iHome.Features.SchedulesList.Service.RemoveSchedule;
using iHome.Features.SchedulesList.Service.UpdateScheduleTime;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.SchedulesList;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSchedulesList(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<GetUserSchedulesOrderedQuery>, GetUserSchedulesOrderedQueryHandler>();
        services.AddScoped<IAsyncCommandHandler<RemoveScheduleCommand>, RemoveScheduleCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<UpdateScheduleTimeCommand>, UpdateScheduleTimeCommandHandler>();
        services.AddScoped<ISchedulesListService, SchedulesListService>();
        return services;
    }
}
