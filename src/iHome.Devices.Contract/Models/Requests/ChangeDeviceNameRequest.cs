namespace iHome.Devices.Contract.Models.Requests;

public class ChangeDeviceNameRequest
{
    public Guid DeviceId { get; set; }
    public string Name { get; set; }
}
