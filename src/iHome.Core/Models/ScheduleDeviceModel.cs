using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Microservices.Devices.Contract.Models;

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
        var d = device.Device;
        if (d == null) return;
        Device = new DeviceModel
        {
            Id = d.Id,
            RoomId = d.RoomId,
            MacAddress = d.MacAddress,
            Data = string.Empty,
            Name = d.Name,
            Type = (DeviceType)d.Type
        };
    }
}
