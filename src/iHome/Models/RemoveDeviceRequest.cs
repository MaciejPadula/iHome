namespace iHome.Models;

public class RemoveDeviceRequest
{
    public required Guid WidgetId { get; set; }
    public required Guid DeviceId { get; set; }
}
