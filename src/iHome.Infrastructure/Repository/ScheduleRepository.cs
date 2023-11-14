using iHome.Infrastructure.Mappers;
using iHome.Microservices.Schedules.Contract;
using iHome.Model;
using iHome.Repository;

namespace iHome.Infrastructure.Repository;

internal class ScheduleRepository : IScheduleRepository
{
    private readonly IScheduleManagementService _scheduleManagementService;

    public ScheduleRepository(IScheduleManagementService scheduleManagementService)
    {
        _scheduleManagementService = scheduleManagementService;
    }

    public async Task Add(ScheduleDto schedule)
    {
        await _scheduleManagementService.AddSchedule(new()
        {
            ScheduleName = schedule.Name,
            Hour = schedule.Hour,
            Minute = schedule.Minute,
            Day = schedule.Day,
            UserId = schedule.UserId
        });
    }

    public async Task<IEnumerable<ScheduleDto>> GetUserSchedules(string userId)
    {
        var response = await _scheduleManagementService.GetSchedules(new()
        {
            UserId = userId
        });

        return response?
            .Schedules?
            .Select(s => s.ToDto()) ?? Enumerable.Empty<ScheduleDto>();
    }

    public async Task Remove(Guid id)
    {
        await _scheduleManagementService.RemoveSchedule(new()
        {
            ScheduleId = id
        });
    }

    public async Task UpdateScheduleTime(Guid id, TimeModel time, int day)
    {
        await _scheduleManagementService.UpdateScheduleTime(new()
        {
            ScheduleId = id,
            Hour = time.Hour,
            Minute = time.Minute,
            Day = day
        });
    }
}
