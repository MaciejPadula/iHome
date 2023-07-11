namespace iHome.Models.Requests.Widgets;

public class RemoveWidgetDeviceRequest
{
    public required Guid WidgetId { get; set; }
    public required Guid DeviceId { get; set; }
}
