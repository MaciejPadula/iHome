namespace iHome.Devices.Contract.Models.Requests;

public class AddDeviceRequest
{
    public string Name { get; set; }
    public string MacAddress { get; set; }
    public DeviceType Type { get; set; }
    public Guid HubId { get; set; }
    public Guid RoomId { get; set; }
}
