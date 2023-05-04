using iHome.Infrastructure.SQL.Models.Enums;
using iHome.Infrastructure.SQL.Models.RootTables;

namespace iHome.Core.Models;

public class WidgetModel
{
    public Guid Id { get; set; }
    public WidgetType WidgetType { get; set; }
    public Guid RoomId { get; set; }
    public bool ShowBorder { get; set; }
    public int MaxNumberOfDevices { get; set; }

    public WidgetModel(Widget widget)
    {
        Id = widget.Id;
        WidgetType = widget.WidgetType;
        RoomId = widget.RoomId;
        ShowBorder = widget.ShowBorder;
        MaxNumberOfDevices = widget.MaxNumberOfDevices;
    }
}
