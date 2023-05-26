using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Models.Requests.Device;

public class AddDeviceRequest
{
    public required string Name { get; set; } = string.Empty;
    public required string MacAddress { get; set; } = string.Empty;
    public required DeviceType Type { get; set; }
    public required Guid RoomId { get; set; }
}
