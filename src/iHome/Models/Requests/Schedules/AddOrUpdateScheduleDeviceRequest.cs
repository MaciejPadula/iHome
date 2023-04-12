namespace iHome.Models.Requests.Schedules;

public class AddOrUpdateScheduleDeviceRequest
{
    public required Guid ScheduleId { get; set; }
    public required Guid DeviceId { get; set; }
    public required string DeviceData { get; set; }
}
