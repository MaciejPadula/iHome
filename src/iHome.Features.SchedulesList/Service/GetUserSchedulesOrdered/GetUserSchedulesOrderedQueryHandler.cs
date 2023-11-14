using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.SchedulesList.Service.GetUserSchedulesOrdered;

internal class GetUserSchedulesOrderedQueryHandler : IAsyncQueryHandler<GetUserSchedulesOrderedQuery>
{
    private readonly IScheduleRepository _scheduleRepository;

    public GetUserSchedulesOrderedQueryHandler(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task HandleAsync(GetUserSchedulesOrderedQuery query)
    {
        var result = await _scheduleRepository.GetUserSchedules(query.UserId);
        query.Result = result
            .OrderBy(s => s.Hour)
            .ThenBy(s => s.Minute)
            .ToList();
    }
}
