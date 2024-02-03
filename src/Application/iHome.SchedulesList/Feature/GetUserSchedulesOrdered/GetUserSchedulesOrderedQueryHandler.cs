using iHome.Microservices.Schedules.Contract;
using iHome.Model;
using iHome.SchedulesList.Feature.Shared.Mappers;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.SchedulesList.Feature.GetUserSchedulesOrdered;

internal class GetUserSchedulesOrderedQueryHandler : IAsyncQueryHandler<GetUserSchedulesOrderedQuery>
{
    private readonly IScheduleManagementService _scheduleManagementService;

    public GetUserSchedulesOrderedQueryHandler(IScheduleManagementService scheduleManagementService)
    {
        _scheduleManagementService = scheduleManagementService;
    }

    public async Task HandleAsync(GetUserSchedulesOrderedQuery query)
    {
        var response = await _scheduleManagementService.GetSchedules(new()
        {
            UserId = query.UserId
        });

        var schedules = response?
            .Schedules?
            .Select(s => s.ToDto()) ?? Enumerable.Empty<ScheduleDto>();

        query.Result = schedules.ToList();
    }
}
