using iHome.Infrastructure.SQL.Models.ConnectionTables;

namespace iHome.Core.Models;

public class ScheduleDeviceModel
{
    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid DeviceId { get; set; }
    public string Name { get; set; }
    public string DeviceData { get; set; }
    public DeviceModel? Device { get; set; }

    public ScheduleDeviceModel(ScheduleDevice device)
    {
        Id = device.Id;
        ScheduleId = device.ScheduleId;
        DeviceId = device.DeviceId;
        Name = device.Device?.Name ?? string.Empty;
        DeviceData = device.DeviceData;
        Device = new DeviceModel(device.Device);
    }
}
