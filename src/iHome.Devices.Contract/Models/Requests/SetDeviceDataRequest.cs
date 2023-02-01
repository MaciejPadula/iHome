namespace iHome.Devices.Contract.Models.Requests;

public class SetDeviceDataRequest
{
    public required Guid DeviceId { get; set; }
    public required string Data { get; set; }
}
