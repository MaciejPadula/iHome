using iHome.Infrastructure.SQL.Models;

namespace iHome.Models.Responses;

public class GetDevicesDevice
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public DeviceType Type { get; init; }
    public string Data { get; set; }
    public Guid RoomId { get; set; }
    public string MacAddress { get; init; }

    public GetDevicesDevice(Device device)
    {
        Id = device.Id;
        Name = device.Name;
        Type = device.Type;
        Data = device.Data;
        RoomId = device.RoomId;
        MacAddress = device.MacAddress;
    }
}
