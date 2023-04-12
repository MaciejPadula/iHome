using iHome.Infrastructure.SQL.Models;

namespace iHome.Models.Responses;

public class GetScheduleDevicesDevice
{
    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceData { get; set; }

    public GetScheduleDevicesDevice(ScheduleDevice device)
    {
        Id = device.Id;
        ScheduleId = device.ScheduleId;
        DeviceId = device.DeviceId;
        DeviceData = device.DeviceData;
    }
}
