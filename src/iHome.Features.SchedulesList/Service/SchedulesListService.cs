using iHome.Features.SchedulesList.Service.GetUserSchedulesOrdered;
using iHome.Features.SchedulesList.Service.RemoveSchedule;
using iHome.Features.SchedulesList.Service.UpdateScheduleTime;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.Features.SchedulesList.Service;

internal class SchedulesListService : ISchedulesListService
{
    private readonly IMediator _mediator;

    public SchedulesListService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<ScheduleDto>> GetUserSchedulesOrdered(string userId)
    {
        var query = await _mediator.HandleQueryAsync(new GetUserSchedulesOrderedQuery
        {
            UserId = userId
        });

        return query.Result;
    }

    public async Task RemoveSchedule(Guid id, string userId)
    {
        await _mediator.HandleCommandAsync(new RemoveScheduleCommand
        {
            Id = id,
            UserId = userId
        });
    }

    public async Task UpdateScheduleTime(Guid id, string time, int day, string userId)
    {
        await _mediator.HandleCommandAsync(new UpdateScheduleTimeCommand
        {
            Id = id,
            Time = time,
            Day = day,
            UserId = userId
        });
    }
}
