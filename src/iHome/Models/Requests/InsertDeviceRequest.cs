namespace iHome.Models.Requests;

public class InsertDeviceRequest
{
    public required Guid WidgetId { get; set; }
    public required Guid DeviceId { get; set; }
}
