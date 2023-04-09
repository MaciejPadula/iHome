using iHome.Infrastructure.SQL.Models;

namespace iHome.Models.Requests;

public class AddWidgetRequest
{
    public Guid RoomId { get; set; }
    public WidgetType WidgetType { get; set; }
    public bool ShowBorder { get; set; }
}