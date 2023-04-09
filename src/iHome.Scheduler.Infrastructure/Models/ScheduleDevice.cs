namespace iHome.Scheduler.Repositories.Models;

public class ScheduleDevice
{
    public Guid Id { get; set; } = new Guid();
    public required Guid ScheduleId { get; set; }
    public required Guid DeviceId { get; set; }
    public required string DeviceData { get; set; }
}
