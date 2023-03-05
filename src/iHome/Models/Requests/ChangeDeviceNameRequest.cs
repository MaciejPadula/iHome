namespace iHome.Models.Requests;

public class ChangeDeviceNameRequest
{
    public required Guid DeviceId { get; set; }
    public required string Name { get; set; } = string.Empty;
}
