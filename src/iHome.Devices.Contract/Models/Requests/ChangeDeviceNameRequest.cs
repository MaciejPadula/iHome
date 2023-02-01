namespace iHome.Devices.Contract.Models.Requests;

public class ChangeDeviceNameRequest
{
    public required Guid DeviceId { get; set; }
    public required string Name { get; set; }
}
