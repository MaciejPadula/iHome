using iHome.Microservices.Widgets.Contract.Models;

namespace iHome.Models.Requests.Widgets;

public class AddWidgetRequest
{
    public Guid RoomId { get; set; }
    public WidgetType WidgetType { get; set; }
    public bool ShowBorder { get; set; }
}