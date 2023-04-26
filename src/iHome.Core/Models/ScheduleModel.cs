using iHome.Infrastructure.SQL.Models;
using iHome.Shared.Logic;

namespace iHome.Core.Models;

public class ScheduleModel
{
    public Guid Id { get; set; } = new Guid();
    public string Name { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public bool Runned { get; set; }
    public IEnumerable<ScheduleDeviceModel> Devices { get; set; }

    public ScheduleModel(Schedule schedule)
    {
        Id = schedule.Id;
        Name = schedule.Name;
        var occurence = CronHelper.GetNextOccurence(schedule.ActivationCron, DateTime.UtcNow);

        if (occurence.HasValue)
        {
            Hour = occurence.Value.Hour;
            Minute = occurence.Value.Minute;
        }

        Devices = schedule.ScheduleDevices?
            .Select(d => new ScheduleDeviceModel(d)) ?? Enumerable.Empty<ScheduleDeviceModel>();

        Runned = Random.Shared.NextDouble() >= 0.5;
    }
}
