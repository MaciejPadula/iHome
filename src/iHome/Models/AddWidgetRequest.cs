using iHome.Core.Models;

namespace iHome.Models;

public class AddWidgetRequest
{
    public Guid RoomId { get; set; }
    public WidgetType WidgetType { get; set; }
    public bool ShowBorder { get; set; }
}
