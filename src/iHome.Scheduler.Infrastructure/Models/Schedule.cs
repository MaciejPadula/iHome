namespace iHome.Scheduler.Repositories.Models;

public class Schedule
{
    public Guid Id { get; set; } = new Guid();
    public required string Name { get; set; }
    public required string ActivationCron { get; set; }
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    public required string UserId { get; set; }

    public virtual ICollection<ScheduleDevice> ScheduleDevices { get; set; } = new List<ScheduleDevice>();
}
