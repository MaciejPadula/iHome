namespace iHome.Model;

public class ScheduleDeviceDto
{
    public Guid Id { get; set; }
    public required Guid DeviceId { get; set; }
    public required Guid ScheduleId { get; set; }
    public required string Data { get; set; }
    public required string Name { get; set; }
    public required int Type { get; set; }
}
