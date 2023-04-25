using iHome.Infrastructure.SQL.Models;

namespace iHome.Models.Responses;

public class GetScheduleDevicesDevice
{
    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid DeviceId { get; set; }
    public string Name { get; set; }
    public string DeviceData { get; set; }
    public GetDevicesDevice? Device { get; set; }

    public GetScheduleDevicesDevice(ScheduleDevice device)
    {
        Id = device.Id;
        ScheduleId = device.ScheduleId;
        DeviceId = device.DeviceId;
        Name = device.Device?.Name ?? string.Empty;
        DeviceData = device.DeviceData;
        Device = new GetDevicesDevice(device.Device);
    }
}
