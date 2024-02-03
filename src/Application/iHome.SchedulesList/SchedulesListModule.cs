using iHome.Microservices.Schedules.Contract;
using iHome.SchedulesList.Feature.AddSchedule;
using iHome.SchedulesList.Feature.GetUserSchedulesOrdered;
using iHome.SchedulesList.Feature.RemoveSchedule;
using iHome.SchedulesList.Feature.UpdateScheduleTime;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Cqrs.Mediator.Command;
using Web.Infrastructure.Cqrs.Mediator.Query;
using Web.Infrastructure.Microservices.Client.Extensions;

namespace iHome.SchedulesList;

public static class SchedulesListModule
{
    public static IServiceCollection AddSchedulesListModule(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryHandler<GetUserSchedulesOrderedQuery>, GetUserSchedulesOrderedQueryHandler>();
        services.AddScoped<IAsyncCommandHandler<RemoveScheduleCommand>, RemoveScheduleCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<UpdateScheduleTimeCommand>, UpdateScheduleTimeCommandHandler>();
        services.AddScoped<IAsyncCommandHandler<AddScheduleCommand>, AddScheduleCommandHandler>();
        services.AddMicroserviceClient<IScheduleManagementService>();
        return services;
    }
}
