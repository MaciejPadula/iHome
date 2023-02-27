namespace iHome.Devices.Contract.Models.Requests;

public class AddDeviceRequest
{
    public string Name { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public Guid HubId { get; set; }
    public Guid RoomId { get; set; }
}
