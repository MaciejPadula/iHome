using iHome.Jobs.Events.Infrastructure.Helpers;

namespace iHome.Jobs.Events.Scheduler.Logic;

public interface IScheduleRunningConditionChecker
{
    bool CheckScheduleRunCondition(int hour, int minute);
}

public class ScheduleRunningConditionChecker : IScheduleRunningConditionChecker
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public ScheduleRunningConditionChecker(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public bool CheckScheduleRunCondition(int hour, int minute)
    {
        var utcNow = _dateTimeProvider.UtcNow;

        return DateTimeExtensions
                .TodaysOccurence(utcNow, hour, minute)
                .EarlierThanOrEqual(utcNow);
    }
}
