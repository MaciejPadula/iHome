namespace iHome.Models.Requests.Device;

public class SetDeviceDataRequest
{
    public Guid DeviceId { get; set; }
    public required string Data { get; set; }
}
