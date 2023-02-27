namespace iHome.Devices.Contract.Models.Requests;

public class SetDeviceDataRequest
{
    public Guid DeviceId { get; set; }
    public string Data { get; set; }
}
