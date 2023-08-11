using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using iHome.Microservices.Schedules.Logic.Helpers;

namespace iHome.Microservices.Schedules.Providers;

public interface ISchedulesRunnedSetter
{
    Task<List<ScheduleModel>> SetRunnedProperty(IEnumerable<ScheduleModel> schedules);
}

public class SchedulesRunnedSetter : ISchedulesRunnedSetter
{
    private readonly IScheduleRunHistoryRepository _scheduleRunHistoryRepository;
    private readonly IClock _clock;

    public SchedulesRunnedSetter(IScheduleRunHistoryRepository scheduleRunHistoryRepository, IClock clock)
    {
        _scheduleRunHistoryRepository = scheduleRunHistoryRepository;
        _clock = clock;
    }

    public async Task<List<ScheduleModel>> SetRunnedProperty(IEnumerable<ScheduleModel> schedules)
    {
        var utcNow = _clock.UtcNow.StartOfDay();
        var result = new List<ScheduleModel>();

        foreach (var schedule in schedules)
        {
            schedule.Runned = await _scheduleRunHistoryRepository.ScheduleRunned(schedule.Id, utcNow);
            result.Add(schedule);
        }

        return result;
    }
}
