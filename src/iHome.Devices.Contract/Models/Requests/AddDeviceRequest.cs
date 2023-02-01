namespace iHome.Devices.Contract.Models.Requests;

public class AddDeviceRequest
{
    public required string Name { get; set; }
    public required string MacAddress { get; set; }
    public required DeviceType Type { get; set; }
    public required string HubId { get; set; }
    public required Guid RoomId { get; set; }
}
