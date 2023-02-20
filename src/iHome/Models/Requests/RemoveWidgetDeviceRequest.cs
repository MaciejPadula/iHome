namespace iHome.Models.Requests;

public class RemoveWidgetDeviceRequest
{
    public required Guid WidgetId { get; set; }
    public required Guid DeviceId { get; set; }
}
