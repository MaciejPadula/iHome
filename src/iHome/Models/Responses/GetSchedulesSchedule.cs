using iHome.Infrastructure.SQL.Models;
using iHome.Shared.Logic;

namespace iHome.Models.Responses;

public class GetSchedulesSchedule
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public IEnumerable<GetScheduleDevicesDevice>? Devices { get; set; }

    public GetSchedulesSchedule(Schedule schedule)
    {
        Id = schedule.Id;
        Name = schedule.Name;
        var occurence = CronHelper.GetNextOccurence(schedule.ActivationCron, DateTime.UtcNow);

        if (occurence.HasValue)
        {
            Hour = occurence.Value.Hour;
            Minute = occurence.Value.Minute;
        }

        if (schedule.ScheduleDevices?.Any() ?? false)
        {
            Devices = schedule.ScheduleDevices.Select(d => new GetScheduleDevicesDevice(d));
        }
    }
}
